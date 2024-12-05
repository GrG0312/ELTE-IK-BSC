using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadandó
{
    public class Kamion
    {
        public class InvalidShaftNumber : Exception { }

        public string rendszám;
        public double terhel;
        private double fogyaszt;
        List<Fuvar> fuvarok;
        Telephely th;
        private ITengely repr;

        public Kamion(string r, double t, double f, Telephely th, int teng, Cég c)
        {
            fuvarok = new List<Fuvar>();
            rendszám = r;
            terhel = t;
            fogyaszt = f;
            this.th = c.Küld(this, th);
            c.Bővül(this);
            switch (teng)
            {
                case 3: repr = Nyerges.Instance(); 
                    break;

                case 2: repr = Fülkés.Instance(); 
                    break;
                default:
                    throw new InvalidShaftNumber();
            }
        }

        public void Vált(Telephely hova)
        {
            hova.Jön(this);
            th.Megy(this);
            th = hova;
        }

        public void Megbízás(double t, double s, int d, DateTime i, int sz, Sofőr sof)
        {
            Fuvar f = new Fuvar(t, s, d, i, sz, sof, this);
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

        public bool Túlterhelt()
        {
            int i = 0;
            while (i < fuvarok.Count())
            {
                if (fuvarok.ElementAt(i).súly > terhel)
                {
                    return true;
                }
                else
                {
                    i += 1;
                }
            }
            return false;
        }

        public double Fogyasztás()
        {
            double sum = 0;
            for (int i = 0; i < fuvarok.Count(); i++)
            {
                if (fuvarok.ElementAt(i).Kész())
                {
                    sum += fogyaszt / 100 * fuvarok.ElementAt(i).táv;
                }
            }
            return sum;
        }

        public void Indul(DateTime idő)
        {
            int i = 0;
            while(i < fuvarok.Count() && fuvarok.ElementAt(i).indul != idő)
            {
                i += 1;
            }
            if (i<fuvarok.Count)
            {
                th.Megy(this);
                fuvarok.ElementAt(i).érkez = fuvarok.ElementAt(i).indul.AddHours(fuvarok.ElementAt(i).szállid);
                fuvarok.ElementAt(i).kézbesítő.Kézbesít(fuvarok.ElementAt(i).indul, fuvarok.ElementAt(i).érkez);
                for (int j = 0; j < fuvarok.Count; j++)
                {
                    if (fuvarok.ElementAt(j).indul >= fuvarok.ElementAt(i).érkez 
                        && fuvarok.ElementAt(j).indul <= fuvarok.ElementAt(i).érkez.AddHours(fuvarok.ElementAt(i).szállid))
                    {
                        Indul(j);
                    }
                }
                th.Jön(this);
            }
        }

        public void Indul(int i)
        {
            fuvarok.ElementAt(i).érkez = fuvarok.ElementAt(i).indul.AddHours(fuvarok.ElementAt(i).szállid);
            fuvarok.ElementAt(i).kézbesítő.Kézbesít(fuvarok.ElementAt(i).indul, fuvarok.ElementAt(i).érkez);
            for (int j = 0; j < fuvarok.Count; j++)
            {
                if (fuvarok.ElementAt(j).indul >= fuvarok.ElementAt(i).érkez
                    && fuvarok.ElementAt(j).indul <= fuvarok.ElementAt(i).érkez.AddHours(fuvarok.ElementAt(i).szállid))
                {
                    Indul(j);
                }
            }
        }

        public int GetTengely()
        {
            return repr.GetTengely();
        }
    }

    interface ITengely
    {
        public int GetTengely() { return 0; }
    }

    public class Nyerges : ITengely
    {
        private static Nyerges instance = null;

        public static Nyerges Instance()
        {
            if (instance == null)
            {
                instance = new Nyerges();
            }
            return instance;
        }
        public int GetTengely()
        {
            return 3;
        }
    }

    public class Fülkés : ITengely
    {
        private static Fülkés instance = null;

        public static Fülkés Instance()
        {
            if (instance == null)
            {
                instance = new Fülkés();
            }
            return instance;
        }
        public int GetTengely()
        {
            return 2;
        }
    }
}
