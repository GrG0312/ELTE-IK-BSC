using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingContest
{
	public class Contest
	{
		public class AlreadyRegisteredException : Exception { }
		public class NotRegisteredException : Exception { }

		public readonly string place;
		public DateTime Start { get; private set; }
		private Organization org;
		private List<Fisher> fishers;

		public Contest(Organization o, string p, DateTime s)
		{
			place = p;
			org = o;
			fishers = new List<Fisher>();
			Start = s;
		}

		public void SignUp(Fisher f)
		{
			if (fishers.Contains(f)) { throw new AlreadyRegisteredException(); }
			if (!org.Members.Contains(f)) { throw new NotRegisteredException(); }
			fishers.Add(f);
		}

		public double TotalAmount()
		{
			double total = 0;
			foreach (Fisher f in fishers)
			{
				total += f.TotalValue(this);
			}
			return total;
		}

		public bool AllCatfish()
		{
			foreach (Fisher f in fishers)
			{
				if (f.CatfishNumber(this) == 0)
				{
					return false;
				}
			}
			return true;
		}
	}
}
