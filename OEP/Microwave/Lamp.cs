using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave
{
	public class Lamp : StateMachine
	{
		private bool on = false;

		public Lamp() : base(Signal.none, Signal.final) { }

		protected override void Transition(Signal signal)
		{
			switch (signal)
			{
				case Signal.closed: case Signal.stopped:
					on = false;
					break;
				case Signal.started: case Signal.opened:
					on = true;
					break;
			}
		}

		public override string ToString() {
			return "lamp is " + (on ? "on" : "off");
		}
	}
}
