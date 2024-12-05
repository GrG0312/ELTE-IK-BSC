using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave
{
	public class Magnetron
	{
		private Door door;
		private Lamp lamp;
		private bool working = false;

		public void Control(Door door, Lamp lamp)
		{
			this.door = door;
			this.lamp = lamp;
		}

		public void Send(Signal signal)
		{
			switch (signal)
			{
				case Signal.pressed:
					if (!working && door.Closed)
					{
						working = true;
						lamp.Send(Signal.started);
					} else if (working) {
						working = false;
						lamp.Send(Signal.stopped);
					}
					break;
				case Signal.opened:
					if (working)
					{
						working = false;
					}
					break;
			}
		}

		public override string ToString()
		{
			return "magnetron is " + (working ? "working" : "not working");
		}
	}
}
