using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadandó
{
    public class Cég
    {
        public class NoSiteFoundException : Exception { }
        public class IncorrectDeliveryDataException : Exception { }

        private List<Telephely> telephelyek;
        private List<Sofőr> alkalmazottak;
        private List<Kamion> kamionok;

        public Cég()
        {
            telephelyek = new List<Telephely>();
            alkalmazottak = new List<Sofőr>();
            kamionok = new List<Kamion>();
        }

        public void Bővül(Telephely t)
        {
            telephelyek.Add(t);
        }

        public void Bővül(Sofőr s)
        {
            alkalmazottak.Add(s);
        }

        public void Bővül(Kamion k)
        {
            kamionok.Add(k);
        }

        public Telephely LegkevesebbK()
        {
            if (telephelyek.Count < 1)
            {
                throw new NoSiteFoundException();
            }
            int min = telephelyek.ElementAt(0).Mennyi();
            Telephely minth = telephelyek.ElementAt(0);
            for (int i = 1; i < telephelyek.Count; i++)
            {
                if (telephelyek.ElementAt(i).Mennyi() < min)
                {
                    min = telephelyek.ElementAt(i).Mennyi();
                    minth = telephelyek.ElementAt(i);
                }
            }
            return minth;
        }

        public double MaxTeher(Telephely t)
        {
            int i = 0;
            while (i< telephelyek.Count() && telephelyek.ElementAt(i).cím != t.cím)
            {
                i++;
            }
            if (i>=telephelyek.Count())
            {
                throw new NoSiteFoundException();
            }
            return telephelyek.ElementAt(i).MaxTeher();
        }

        public void ÚjFuvar(double táv, double súly, int díj, DateTime indul, int szállid, Sofőr sof, Kamion kam)
        {
            bool ls = false;
            bool lk = false;
            Sofőr s = null; Kamion k = null;
            int i = 0;
            while (i < alkalmazottak.Count() && !ls)
            {
                if (alkalmazottak.ElementAt(i).név == sof.név)
                {
                    ls = true;
                    s = alkalmazottak.ElementAt(i);
                } else
                {
                    i++;
                }
            }
            i = 0;
            while (i < kamionok.Count() && !lk)
            {
                if (kamionok.ElementAt(i).rendszám == kam.rendszám)
                {
                    lk = true;
                    k = kamionok.ElementAt(i);
                }
                else { i++; }
            }
            if (!ls || !lk)
            {
                throw new IncorrectDeliveryDataException();
            }
            if (s.Szabad(indul) && k.Szabad(indul))
            {
                s.Megbízás(táv, súly, díj, indul, szállid, k);
                k.Megbízás(táv, súly, díj, indul, szállid, s);
            } else throw new IncorrectDeliveryDataException();
        }

        public void ÚjFuvar(double táv, double súly, int díj, DateTime indul, int szállid, string név, string rend)
        {
            bool ls = false;
            bool lk = false;
            Sofőr s = null; Kamion k = null;
            int i = 0;
            while (i < alkalmazottak.Count() && !ls)
            {
                if (alkalmazottak.ElementAt(i).név == név)
                {
                    ls = true;
                    s = alkalmazottak.ElementAt(i);
                }
                else
                {
                    i++;
                }
            }
            i = 0;
            while (i < kamionok.Count() && !lk)
            {
                if (kamionok.ElementAt(i).rendszám == rend)
                {
                    lk = true;
                    k = kamionok.ElementAt(i);
                }
                else { i++; }
            }
            if (!ls || !lk)
            {
                throw new IncorrectDeliveryDataException();
            }
            if (s.Szabad(indul) && k.Szabad(indul))
            {
                s.Megbízás(táv, súly, díj, indul, szállid, k);
                k.Megbízás(táv, súly, díj, indul, szállid, s);
            }
            else throw new IncorrectDeliveryDataException();
        }

        public bool VoltTúlterhelt()
        {
            int i = 1;
            while (i < kamionok.Count)
            {
                if (kamionok.ElementAt(i).Túlterhelt())
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

        public double Nyereség()
        {
            double fogyasztás = 0;
            foreach (Kamion k in kamionok)
            {
                fogyasztás += k.Fogyasztás();
            }
            double összdíj = 0;
            double bérek = 0;
            foreach (Sofőr s in alkalmazottak)
            {
                összdíj += s.DíjBegyűjt();
                bérek += s.SzámolBér();
            }
            return összdíj - bérek - fogyasztás;
        }

        public Telephely Küld(Kamion k, Telephely th)
        {
            int i = 0;
            bool lt = false;
            while (i < telephelyek.Count() && !lt)
            {
                if (telephelyek.ElementAt(i).cím == th.cím)
                {
                    lt = true;
                }
                else
                {
                    i++;
                }
            }
            if (!lt)
            {
                throw new NoSiteFoundException();
            }
            telephelyek.ElementAt(i).Jön(k);
            return telephelyek[i];
        }

        public void Start(DateTime idő)
        {
            foreach (Kamion k in kamionok)
            {
                k.Indul(idő);
            }
        }
    }
}
