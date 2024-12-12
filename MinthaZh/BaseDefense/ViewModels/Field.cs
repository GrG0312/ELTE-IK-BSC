using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDefense.ViewModels
{
    public class Field
    {
        public int X { get; }
        public int Y { get; }
        public (int, int) Coord { get { return (X, Y); } }
        public Brush BackgroundColor { get; set; }

        public RelayCommand<object> ClickCommand { get; private set; }
        public Field(int X, int Y, Action<int, int> action)
        {
            this.X = X;
            this.Y = Y;
            BackgroundColor = new SolidColorBrush(Avalonia.Media.Color.Parse("White"));
            ClickCommand = new RelayCommand<object>((object? param) =>
            {
                (int X, int Y) cp = ((int, int))param!;
                action(cp.X, cp.Y);
            });
        }
    }
}
