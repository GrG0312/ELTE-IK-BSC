using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDefense.Model
{
    public struct FieldValueEventArgs
    {
        public int X { get; }
        public int Y { get; }
        public FieldType Value { get; }

        public FieldValueEventArgs(int x, int y, FieldType value)
        {
            X = x;
            Y = y;
            Value = value;
        }
    }
}
