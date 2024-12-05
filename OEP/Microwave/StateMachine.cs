using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave
{
	public abstract class StateMachine
	{
		private readonly MyQueue signalQueue;
		private readonly Signal finalSignal;
		private readonly Thread thread;
		private bool processing;

		public StateMachine(Signal none, Signal final)
		{
			finalSignal = final;
			signalQueue = new(none);
			thread = new Thread(new ThreadStart(Process));
			thread.Start();
		}

		public void Send(Signal signal)
		{
			signalQueue.Enqueue(signal);
		}

		private void Process()
		{
			processing = true;
			while (processing)
			{
				try
				{
					Signal signal = signalQueue.Dequeue();
					if (signal == finalSignal)
					{
						processing = false;
					}
					else
					{
						Transition(signal);
					}
				}
				catch (System.InvalidOperationException) { }
			}
		}

		protected abstract void Transition(Signal signal);
	}
}
