using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWar
{
	public class SolarSystem
	{
		public List<Planet> planets;

		public SolarSystem(string fn)
		{
			planets = new List<Planet>();
			TextFile.TextFileReader reader = new(fn);
			while (reader.ReadString(out string planet))
			{
				planets.Add(new Planet(planet));
			}
		}

		public bool MaxFire(out StarShip s)
		{
			s = null!;
			bool l = false;
			double fp = 0.0;
			foreach (Planet p in planets) {
				bool found = p.Firepower(out double firepower, out StarShip ship);
				if (found)
				{
					if (!l)
					{
						l = true;
						s = ship;
						fp = firepower;
					} else
					{
						if (fp < firepower)
						{
							s = ship;
							fp = firepower;
						}
					}
				}
			}
			return l;
		}

		public List<Planet> Unprotected()
		{
			List<Planet> list = new();
			foreach (Planet p in planets)
			{
				if (p.ShipCount() == 0)
				{
					list.Add(p);
				}
			}
			return list;
		}

		public Planet this[string n]
		{
			get
			{
				foreach (Planet p in planets)
				{
					if (p.name == n)
					{
						return p;
					}
				}
				return null!;
			}
		}
	}
}
