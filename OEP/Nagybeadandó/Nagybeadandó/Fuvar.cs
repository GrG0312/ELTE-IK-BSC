using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadandó
{
    public class Fuvar
    {
        public double táv, súly;
        public int díj, szállid;
        public DateTime indul, érkez;
        public Sofőr kézbesítő;
        public Kamion kamion;

        public Fuvar(double t, double s, int d, DateTime i, int sz, Sofőr kiszállító, Kamion kamion)
        {
            táv = t;
            súly = s;
            díj = d;
            indul = i;
            szállid = sz;
            érkez = new DateTime(9999, 12, 30, 23, 59, 59);
            this.kézbesítő = kiszállító;
            this.kamion = kamion;
        }

        public bool Kész()
        {
            if (érkez == new DateTime(9999, 12, 30, 23, 59, 59))
            {
                return false;
            }
            return true;
        }
    }
}
