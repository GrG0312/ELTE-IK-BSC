using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BlackHole.ViewModel
{
    public class BlackHoleField : ViewModelBase
    {
        private int x, y;
        private Microsoft.Maui.Graphics.Color color = Colors.White;
        private bool enabled;

        public Tuple<int, int> XY { get { return new Tuple<int, int>(x, y); } }
        public int X
        {
            get { return x; }
            set
            {
                if (x != value)
                {
                    x = value;
                    OnPropertyChanged(nameof(X));
                    OnPropertyChanged(nameof(XY));
                }
            }
        }
        public int Y
        {
            get { return y; }
            set
            {
                if (y != value)
                {
                    y = value;
                    OnPropertyChanged(nameof(Y));
                    OnPropertyChanged(nameof(XY));
                }
            }
        }
        public Microsoft.Maui.Graphics.Color Colour
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    color = value;
                    OnPropertyChanged(nameof(Colour));
                }
            }
        }
        public DelegateCommand? Click { get; set; }
        public bool IsEnabled
        {
            get { return enabled; }
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    OnPropertyChanged(nameof(IsEnabled));
                }
            }
        }
    }
}
