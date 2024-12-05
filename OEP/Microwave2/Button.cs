using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave
{
	public class Button
	{
		private Magnetron magn;

		public void Control(Magnetron magn)
		{
			this.magn = magn;
		}

		public void Press()
		{
			magn.Send(Signal.pressed);
		}
	}
}
