using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Model.CustomEventArgs
{
    /// <summary>
    /// Use this EventArgs when you want to pass on a coordinate and a T type value
    /// </summary>
    /// <typeparam name="T">The class of the object that is associated with the coordinate</typeparam>
    public class PlaceValueEventArgs<T> : PlaceEventArgs
    {
        /// <summary>
        /// The object which is associated with the coordinate
        /// </summary>
        public T Value { get; private set; }

        public PlaceValueEventArgs(int x, int y, T v) : base(x,y)
        {
            Value = v;
        }
    }
}
