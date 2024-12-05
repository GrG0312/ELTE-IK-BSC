using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingContest
{
	public class Catch
	{
		public DateTime Time { get; private set; }
		private readonly double weight;
		public Fish Fish { get; private set; } 
		public Contest Contest { get; private set; }
		public Fisher Fisher { get; private set; }

		public Catch(DateTime t, Fish f, double w, Fisher fi, Contest c)
		{
			Time = t;
			weight = w;
			Fish = f;
			Contest = c;
			Fisher = fi;
		}

		public double Value()
		{
			return weight * Fish.Point();
		}
	}
}
