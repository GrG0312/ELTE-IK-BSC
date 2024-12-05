using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextFile;

namespace Horgasz
{
	public class InFile
	{
		private TextFileReader reader;
		public InFile(string fn) {
			reader = new TextFileReader(fn);
		}

		public bool Read(out Horgasz h)
		{
			h = null;
			bool ok = reader.ReadLine(out string line);
			if (ok)
			{
				string[] strings = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				h = new Horgasz(strings[0]);
				for (int i = 1; i < strings.Length; i+=4)
				{
					h.Add(new Horgasz.Fogas(
							strings[i],
							strings[i+1],
							double.Parse(strings[i+2]),
							double.Parse(strings[i+3])
						));
				}
			}
			return ok;
		}
	}
}
