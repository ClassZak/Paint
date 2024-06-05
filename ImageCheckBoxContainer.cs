using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    internal class ImageCheckBoxContainer
    {
        bool _selected = false;
        public void Click()
        {
            _selected=!_selected;
        }
        public bool IsSelected()
        {
            return _selected;
        }
    }
}
