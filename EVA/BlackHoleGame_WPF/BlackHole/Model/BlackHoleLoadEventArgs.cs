using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHole.Model
{
    public class BlackHoleLoadEventArgs
    {
        public int p1 { get; private set; }
        public int p2 { get; private set; }
        public int size { get; private set; }

        public BlackHoleLoadEventArgs(int p1, int p2, int size)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.size = size;
        }
    }
}
