using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paint
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BrushType TypeOfBrush= Paint.BrushType.Point;
        public MainWindow()
        {
            InitializeComponent();
            this.DrawingField.DefaultDrawingAttributes.Width = 5;
            this.DrawingField.DefaultDrawingAttributes.Height =5;
        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            DrawingField.Strokes.Clear();
        }



        private void Button_Save(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog=new SaveFileDialog();
            saveFileDialog.Title = "Сохраните файл";
            bool? res=saveFileDialog.ShowDialog();


            if (res is null)
                return;
            if (res == false)
                return;

            if (saveFileDialog.FileName is null)
                return;
            else
                if (saveFileDialog.FileName == "")
                return;
            FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
            DrawingField.Strokes.Save(fileStream);
            fileStream.Close();
        }

        private void Button_Open(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog =new OpenFileDialog();
            openFileDialog.Filter = "Ink файлы (*.ink)|*.ink";
            bool? res=openFileDialog.ShowDialog();

            if (res is null)
                return;
            if (res == false)
                return;
            FileStream fileStream=new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
            DrawingField.Strokes=new System.Windows.Ink.StrokeCollection(fileStream);
            fileStream.Close();
        }

        private void ColorPicker_ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Color c = (Color)e.NewValue;
            if (DrawingField is null)
                return;
            DrawingField.DefaultDrawingAttributes.Color = Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        private void SizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SizeSlider is null || DrawingField is null)
                return;

            SetBrushSize(SizeSlider.Value, this.TypeOfBrush);
        }






        //private
        #region Brush types
        private void SetBrushPoint(double size)
        {
            SetBrushPoint((uint)Math.Truncate(size));
        }
        private void SetBrushPoint(uint size)
        {
            DrawingField.DefaultDrawingAttributes.Height = size;
            DrawingField.DefaultDrawingAttributes.Width = size;

            this.SizeTextBlock.Text = size.ToString();
        }


        private void SetBrushHor(double size)
        {
            SetBrushHor((uint)Math.Truncate(size));
        }
        private void SetBrushHor(uint size)
        {
            DrawingField.DefaultDrawingAttributes.Height = 1;
            DrawingField.DefaultDrawingAttributes.Width = size;

            this.SizeTextBlock.Text = size.ToString();
        }


        private void SetBrushVert(double size)
        {
            SetBrushVert((uint)Math.Truncate(size));
        }
        private void SetBrushVert(uint size)
        {
            DrawingField.DefaultDrawingAttributes.Height = size;
            DrawingField.DefaultDrawingAttributes.Width = 1;

            this.SizeTextBlock.Text = size.ToString();
        }




        private void SetBrushSize(double size, BrushType brushType)
        {
            SetBrushSize((uint)Math.Truncate(size), brushType);
        }
        private void SetBrushSize(uint size, BrushType brushType)
        {
            this.TypeOfBrush = brushType;
            switch (brushType)
            {
                case Paint.BrushType.Point:
                    SetBrushPoint(size);
                    break;
                case Paint.BrushType.Vert:
                    SetBrushVert(size);
                    break;
                case Paint.BrushType.Hor:
                    SetBrushHor(size);
                    break;
            }
        }

        #endregion

        private void BrushTypeList_MouseLeave(object sender, MouseEventArgs e)
        {
            if (SizeSlider is null || BrushTypeList is null)
                return;


            if (ModeListBox.SelectedIndex==1)
            ModeListBox.SelectedIndex = 0;

            SetBrushSize(SizeSlider.Value, (BrushType)(BrushTypeList.SelectedIndex));
            ModeListBox_MouseLeave(EraserImage, null);
        }



        private void BrushTypeImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ModeListBox_MouseLeave(EraserImage, null);
        }

        private void ModeListBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if(ModeListBox.SelectedIndex==0)
            {
                DrawingField.EditingMode = InkCanvasEditingMode.Ink;
                SetBrushSize(SizeSlider.Value, (BrushType)(BrushTypeList.SelectedIndex));
            }
            else
            {
                DrawingField.EditingMode = InkCanvasEditingMode.EraseByPoint;
            }
        }
    }
}
