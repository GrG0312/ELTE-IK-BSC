using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave
{
	public enum Signal { none, final, started, stopped, pressed, opened, closed };

	public class Micro
	{
		public readonly Button button = new();
		public readonly Door door = new();
		private readonly Magnetron magn = new();
		private readonly Lamp lamp = new();

		public Micro()
		{
			button.Control(magn);
			door.Control(magn, lamp);
			magn.Control(door, lamp);
		}

		public void Stop()
		{
			lamp.Send(Signal.final);
			magn.Send(Signal.final);
		}

		public void Write() {
			Console.WriteLine(lamp);
			Console.WriteLine(door);
			Console.WriteLine($"{magn}\n---");
		}
	}
}
