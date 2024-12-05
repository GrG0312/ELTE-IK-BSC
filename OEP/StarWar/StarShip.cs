using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWar
{
	public abstract class StarShip
	{
		public class NullPlanetException : Exception { }

		protected string name;
		public int Shield { get; protected set; }
		protected int armor, crew;
		private Planet planet;

		public StarShip(string n, int s, int a, int c)
		{
			name = n;
			Shield = s;
			crew = a;
			armor = c;
		}

		public void ProtectPlanet(Planet p)
		{
			if (p == null)
			{
				throw new NullPlanetException();
			}
			planet = p;
			planet.Protect(this);
		}

		public void AbandonPlanet(Planet p)
		{
			planet.Abandon(this);
			planet = null!;
		}

		public abstract double Firepower();
	}

	public class Breaker : StarShip
	{
		public Breaker(string n, int s, int a, int c) : base(n, s, a, c) { }

		public override double Firepower()
		{
			return (double)armor / 2;
		}
	}

	public class Lander : StarShip
	{
		public Lander(string n, int s, int a, int c) : base(n, s, a, c) { }

		public override double Firepower()
		{
			return crew;
		}
	}

	public class Laser : StarShip
	{
		public Laser(string n, int s, int a, int c) : base(n, s, a, c) { }

		public override double Firepower()
		{
			return Shield;
		}
	}
}
