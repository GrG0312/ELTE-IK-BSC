using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingContest
{
	public class Fisher
	{
		public class ExistingCatchException : Exception { }

		public readonly string name;
		private List<Catch> catches;

		public Fisher(string name)
		{
			this.name = name;
			catches = new List<Catch>();
		}

		public void Catch(DateTime d, Fish f, double w, Contest c)
		{
			bool l = false;
			foreach (Catch ca in catches)
			{
				if (ca.Time.Equals(d) && ca.Contest.Equals(c)) {
					l = true;
					break;
				}
				if (l)
				{
					throw new ExistingCatchException();
				}
			}
			catches.Add(new FishingContest.Catch(d, f, w, this, c));
		}

		public double TotalValue(Contest c)
		{
			double s = 0;
			foreach (Catch ca in catches)
			{
				if (ca.Contest == c)
				{
					s += ca.Value();
				}
			}
			return s;
		}

		public int CatfishNumber(Contest c)
		{
			int number = 0;
			foreach (Catch ca in catches)
			{
				if (ca.Fish.IsCatfish() && ca.Contest == c)
				{
					number++;
				}
			}
			return number;
		}
	}
}
