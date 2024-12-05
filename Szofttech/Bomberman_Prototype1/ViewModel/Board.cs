using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Bomberman_Prototype1.ViewModel
{
    public class Board : ViewModelBase
    {
        private int width;
        private int height;
        public int Width { get { return width; } set { width = value; OnPropertyChanged(); } }
        public int Height { get { return height; } set { height = value; OnPropertyChanged(); } }
        private Brush groundcolor = new SolidColorBrush(Colors.Indigo);
        public Brush Groundcolor { get { return groundcolor; } set { groundcolor = value; OnPropertyChanged(); } }
        public Board(int width, int height, SolidColorBrush color)
        {
            Width = width;
            Height = height;
            Groundcolor = color;
        }

        public override string ToString()
        {
            return $"{width};{height};{groundcolor};";
        }
    }
}
