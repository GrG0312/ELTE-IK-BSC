using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Horgasz
{
	public class Horgasz
	{
		public class Fogas {
			public readonly string datum, fajta;
			public readonly double tomeg, hossz;

			public Fogas(string d, string f, double t, double h)
			{
				datum = d;
				fajta = f;
				tomeg = t;
				hossz = h;
			}
		}

		public readonly string nev;
		private List<Fogas> fogasok;

		public Horgasz(string n)
		{
			nev = n;
			fogasok = new List<Fogas>();
		}

		public void Add(Fogas f)
		{
			fogasok.Add(f);
		}

		public bool Megfelel()
		{
			int harcsak = 0;
			bool voltPonty = false;
			foreach (Fogas f in fogasok)
			{
				if (voltPonty && f.fajta == "harcsa" && f.hossz >= 1.0)
				{
					harcsak++;
				}
				if (!voltPonty && f.fajta == "ponty" && f.tomeg >= 1.0)
				{
					voltPonty = true;
				}
			}
			return harcsak >= 4;
		}
	}
}
