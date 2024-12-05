using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace pingvinek
{
    public class Pingvin
    {
        public readonly string fajta;
        public readonly string származás;
        public readonly int darab;

        public Pingvin(string f, string sz, int db)
        {
            fajta = f;
            származás = sz;
            darab = db;
        }
    }
    public class Megfigyelés
    {
        public readonly string dátum;
        public readonly int becslés;
        public readonly List<Pingvin> pingvinek;

        public Megfigyelés(string d,  int becsl)
        {
            dátum = d;
            becslés = becsl;
            pingvinek = new List<Pingvin>();
        }

        public void Add(Pingvin p)
        {
            pingvinek.Add(p);
        }

        public int Össz(List<Pingvin> p)
        {
            int s = 0;
            for (int i = 0; i < p.Count; i++)
            {
                s += p[i].darab;
            }
            return s;
        }
    }
}
