using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave
{
	public class MyQueue
	{
		private readonly Signal noneSignal;

		public MyQueue(Signal none)
		{
			this.noneSignal = none;
		}

		private readonly Queue<Signal> queue = new Queue<Signal>();

		private readonly object critical = new object();

		public bool IsEmpty()
		{
			return queue.Count == 0;
		}

		public void Enqueue(Signal s)
		{
			Monitor.Enter(critical);
			queue.Enqueue(s);
			Monitor.Exit(critical);
		}

		public Signal Dequeue()
		{
			Signal s;
			Monitor.Enter(critical);
			if (!IsEmpty())
			{
				s = queue.Dequeue();
			} else
			{
				s = noneSignal;
			}
			Monitor.Exit(critical);
			return s;
		}
	}
}
