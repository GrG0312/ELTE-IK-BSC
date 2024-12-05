using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace StarWar
{
	public class Planet
	{
		public string name { get; private set; }
		private List<StarShip> ships;

		public Planet(string n)
		{
			name = n;
			ships = new List<StarShip>();
		}

		public int ShipCount()
		{
			return ships.Count;
		}

		public void Protect(StarShip s)
		{
			ships.Add(s);
		}

		public void Abandon(StarShip s)
		{
			ships.Remove(s);
		}

		public double TotalShield()
		{
			double sum = 0.0;
			foreach (StarShip s in ships)
			{
				sum += s.Shield;
			}
			return sum;
		}

		public bool Firepower(out double max, out StarShip s)
		{
			max = 0.0;
			bool l = false;
			s = null!;
			foreach (StarShip ship in ships)
			{
				if (!l)
				{
					s = ship;
					max = ship.Firepower();
					l = true;
				} else
				{
					if (max < ship.Firepower())
					{
						s = ship;
						max = ship.Firepower();
					}
				}
			}
			return l;
		}
	}
}
