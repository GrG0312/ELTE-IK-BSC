using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Model.CustomEventArgs
{
    /// <summary>
    /// Use this EventArgs when you want to pass a coordinate
    /// </summary>
    public class PlaceEventArgs : EventArgs
    {
        /// <summary>
        /// The X value of the coordinate
        /// </summary>
        public int Col { get; private set; }
        /// <summary>
        /// The Y value of the coordinate
        /// </summary>
        public int Row { get; private set; }

        public PlaceEventArgs(int col, int row)
        {
            Col = col;
            Row = row;
        }
    }
}
