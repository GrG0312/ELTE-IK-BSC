using System;

namespace StarWar
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				SolarSystem s = new SolarSystem("solarsystem.txt");
				Read(s, "input.txt");
				bool found = s.MaxFire(out StarShip st);
				if (found)
				{
					Console.WriteLine($"A legnagyobb erejű csillaghajó ereje {st.Firepower()}");
				}
				else
				{
					Console.WriteLine("Nincs védett bolygó!");
				}
				Console.WriteLine("Védelem nélküli bolygók:");
				foreach (Planet p in s.Unprotected())
				{
					Console.WriteLine(p.name);
				}
				Planet earth = s["Earth"];
				Console.WriteLine($"A Föld összes pajzsa: {earth.TotalShield()}");
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("Nincs bemeneti fájl!");
			}
			catch (StarShip.NullPlanetException)
			{
				Console.WriteLine("Nem létező bolygó!");
			}
		}

		static void Read(SolarSystem so, string fn)
		{
			TextFile.TextFileReader reader = new(fn);
			reader.ReadInt(out int planets);
			for (int i = 0; i < planets; i++)
			{
				reader.ReadString(out string planetName);
				reader.ReadInt(out int ships);
				reader.ReadLine(out string nothing);
				for (int j = 0; j < ships; j++)
				{
					reader.ReadLine(out string line);
					string[] strings = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					StarShip st = null!;
					int s = int.Parse(strings[2]),
						a = int.Parse(strings[3]),
						c = int.Parse(strings[4]);
					switch (strings[1])
					{
						case "Breaker":
							st = new Breaker(strings[0], s, a, c);
							break;
						case "Lander":
							st = new Lander(strings[0], s, a, c);
							break;
						case "Laser":
							st = new Laser(strings[0], s, a, c);
							break;
					}
					so[planetName].Protect(st);
				}
			}
		}
	}
}