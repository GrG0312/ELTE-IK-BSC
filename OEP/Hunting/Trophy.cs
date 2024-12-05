using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunting
{
	public class Trophy
	{
		private string location, date;
		public Animal Kill { get; private set; }

		public Trophy(string l, string d, Animal a)
		{
			location = l;
			date = d;
			Kill = a;
		}
	}
}
