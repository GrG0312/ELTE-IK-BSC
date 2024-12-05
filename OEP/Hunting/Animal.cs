using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunting
{
	public abstract class Animal
	{
		public int Weight { get; private set; }
		public bool Male { get; private set; }

		protected Animal(int w, bool m) {
			Weight = w;
			Male = m;
		}

		public virtual bool IsElephant() { return false; }
		public virtual bool IsRhino() { return false; }
		public virtual bool IsLion() { return false; }
	}

	public class Elephant : Animal
	{
		public int Right { get; private set; }
		public int Left { get; private set; }

		public Elephant(int w, bool m, int r, int l) : base(w, m) {
			Right = r;
			Left = l;
		}

		public override bool IsElephant() { return true; }
	}

	public class Rhino : Animal
	{
		public int Horn { get; private set; }

		public Rhino(int w, bool m, int h) : base(w, m) {
			Horn = h;
		}

		public override bool IsRhino() { return true; }
	}

	public class Lion : Animal
	{
		public Lion(int w, bool m) : base(w, m) { }

		public override bool IsLion() { return true; }
	}
}
