using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextFile;

namespace ZhGyak
{
    public class Megfigyelés
    {
        public class Pingvin {
            private string faj, szarmazas;
            public int darab { get; private set; }

            public Pingvin(string faj, string szarmazas, int darab)
            {
                this.faj = faj;
                this.szarmazas = szarmazas;
                this.darab = darab;
            }
        }

        private string datum;
        public int becsült { get; private set; }
        public int összes { get; private set; }

        public Megfigyelés(string d, int b) {
            datum = d;
            becsült = b;
            összes = 0;
        }

        public void össz(Pingvin p) {
            összes += p.darab;
        }
        
    }
}
