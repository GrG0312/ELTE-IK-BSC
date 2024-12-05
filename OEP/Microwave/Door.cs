using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave
{
	public class Door
	{
		public bool Closed { get; private set; } = true;
		private Magnetron magn;
		private Lamp lamp;

		public void Control(Magnetron magn, Lamp lamp)
		{
			this.lamp = lamp;
			this.magn = magn;
		}

		public void Open()
		{
			lamp.Send(Signal.opened);
			magn.Send(Signal.opened);
			Closed = false;
		}

		public void Close()
		{
			lamp.Send(Signal.closed);
			Closed = true;
		}

		public override string ToString()
		{
			return "door is " + (Closed ? "closed" : "opened");
		}
	}
}
