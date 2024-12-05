using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadandó
{
    public abstract class Sofőr
    {
        public string név;
        private List<Fuvar> fuvarok;

        public Sofőr(string n)
        {
            név = n;
            fuvarok = new List<Fuvar>();
        }

        protected virtual int Együttható(Fuvar e)
        {
            return 0;
        }

        public void Megbízás(double t, double s, int d, DateTime i, int sz, Kamion kamion)
        {
            Fuvar f = new Fuvar(t, s, d, i, sz, this, kamion);
            fuvarok.Add(f);
        }

        public bool Szabad(DateTime indul)
        {
            for (int i = 0; i < fuvarok.Count; i++)
            {
                if (fuvarok.ElementAt(i).indul == indul)
                {
                    return false;
                }
            }
            return true;
        }

        public void Kézbesít(DateTime indul, DateTime érkez)
        {
            int i = 0;
            while (fuvarok.ElementAt(i).indul != indul)
            {
                i++;
            }
            fuvarok.ElementAt(i).érkez = érkez;
        }

        public double SzámolBér()
        {
            double bér = 0;
            for (int i = 0; i < fuvarok.Count; i++)
            {
                if (fuvarok.ElementAt(i).Kész())
                {
                    bér += fuvarok.ElementAt(i).táv * Együttható(fuvarok.ElementAt(i));
                }
            }
            return bér;
        }

        public double DíjBegyűjt()
        {
            int díj = 0;
            for(int i = 0;i < fuvarok.Count;i++)
            {
                if (fuvarok.ElementAt(i).Kész())
                {
                    díj += fuvarok.ElementAt(i).díj;
                }
            }
            return Convert.ToDouble(díj);
        }
    }

    public class Törzstag : Sofőr
    {
        public Törzstag(string n) : base(n) { }

        protected override int Együttható(Fuvar e)
        {
            return 40;
        }
    }

    public class Gyakorlott : Sofőr
    {
        public Gyakorlott(string n) : base(n) { }

        protected override int Együttható(Fuvar e)
        {
            return 30 + (e.kamion.GetTengely() == 3 ? 5 : 0);
        }
    }

    public class Kezdő : Sofőr
    {
        public Kezdő(string n) : base(n) { }

        protected override int Együttható(Fuvar e)
        {
            return 20 + (e.kamion.GetTengely() == 3 ? 5 : 0);
        }
    }
}
