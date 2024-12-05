using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingContest
{
	public class Organization
	{
		public class AlreadyMemberException : Exception { }
		public class ExistingContestExcpetion : Exception { }

		public List<Fisher> Members { get; private set; } = new();
		private List<Contest> contests = new();

		public Fisher Join(string name)
		{
			if (Search(name) != null)
			{
				throw new AlreadyMemberException();
			}
			Fisher fisher = new Fisher(name);
			Members.Add(fisher);
			return fisher;
		}

		public Fisher Search(string name)
		{
			foreach (Fisher f in Members) {
				if (f.name == name)
				{
					return f;
				}
			}
			return null;
		}

		public Contest Organize(string p, DateTime s)
		{
			bool l = false;
			foreach (Contest c in contests) {
				if (c.Start == s && c.place == p)
				{
					l = true;
					break;
				}
			}
			if (l) throw new ExistingContestExcpetion();
			Contest co = new Contest(this, p, s);
			contests.Add(co);
			return co;
		}

		public bool BestContest(out Contest contest)
		{
			bool l = false;
			contest = null!;
			double max = 0;
			foreach (Contest c in contests)
			{
				if (c.AllCatfish())
				{
					double s = c.TotalAmount();
					if (l)
					{
						if (s > max)
						{
							contest = c;
							max = s;
						}
					} else
					{
						contest = c;
						max = s;
						l = true;
					}
				} else 
				{
					// skip
				}
			}
			return l;
		}
	}
}
