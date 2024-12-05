using System;

namespace Horgasz
{
	class Program
	{
		static void Main(string[] args)
		{
			List<Horgasz> horgaszok = Szamol("input.txt");
			foreach (Horgasz h in horgaszok)
			{
				Console.WriteLine(h.nev);
			}
		}

		public static List<Horgasz> Szamol(string fn)
		{
			List<Horgasz> horgaszok = new List<Horgasz>();
			Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
			try
			{
				InFile file = new InFile(fn);
				while (file.Read(out Horgasz h))
				{
					if (h.Megfelel())
					{
						horgaszok.Add(h);
					}
				}
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("Nincs bemeneti fájl!");
			}
			return horgaszok;
		}
	}
}