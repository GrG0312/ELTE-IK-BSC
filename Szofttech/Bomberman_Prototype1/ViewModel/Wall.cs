using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.ViewModel
{
    public class Wall : ViewModelBase
    {
        private int _xpos;
        private int _ypos;
        private int _size;
        private string? _type;
        private string? _spriteUri;
        public int X { get { return _xpos; } set { _xpos = value; OnPropertyChanged(); } }
        public int Y { get { return _ypos; } set { _ypos = value; OnPropertyChanged(); } }
        public int Size { get { return _size; } set { _size = value; OnPropertyChanged(); } }
        public string Type { get { return _type!; } set { _type = value; OnPropertyChanged(); } }
        public string SpriteUri { get { return _spriteUri!; } set { _spriteUri = value; OnPropertyChanged(); } }
        public Wall(int xpos, int ypos, int size, string type, string spriteUri)
        {
            X = xpos;
            Y = ypos;
            Size = size;
            Type = type;
            SpriteUri = spriteUri;
        }

        public override string ToString()
        {
            return $"{_xpos};{_ypos};{_size};{_type};{_spriteUri};";
        }
    }
}
