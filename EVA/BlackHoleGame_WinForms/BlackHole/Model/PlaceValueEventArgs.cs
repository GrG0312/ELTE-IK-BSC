using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlackHole.Model
{
    public class PlaceValueEventArgs
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Value {  get; private set; }

        public PlaceValueEventArgs(int x, int y, int value)
        {
            X = x;
            Y = y;
            Value = value;
        }
    }
}
