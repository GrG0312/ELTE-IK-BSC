using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunting
{
	public class Hunter
	{
		private string name, birth;
		private List<Trophy> trophies;

		public Hunter(string n, string b)
		{
			name = n;
			birth = b;
			trophies = new List<Trophy>();
		}

		public void Kill(string l, string w, Animal a)
		{
			trophies.Add(new Trophy(l, w, a));
		}

		public int MaleLions()
		{
			int count = 0;
			foreach (Trophy t in trophies)
			{
				if (t.Kill.IsLion() && t.Kill.Male)
				{
					count++;
				}
			}
			return count;
		}

		public bool MaxHornWeight(out double ratio)
		{
			bool l = false;
			ratio = 0.0;
			foreach (Trophy t in trophies )
			{
				if (t.Kill.IsRhino())
				{
					Rhino r = (Rhino)t.Kill;
					double ra = (double)r.Horn / r.Weight;
					if (l == false)
					{
						ratio = ra;
						l = true;
					} else
					{
						if (ratio < ra)
						{
							ratio = ra;
						}
					}
				}
			}
			return l;
		}

		public bool SameFangLength()
		{
			foreach (Trophy t in trophies)
			{
				if (t.Kill.IsElephant())
				{
					Elephant e = (Elephant)t.Kill;
					if (e.Left == e.Right)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
