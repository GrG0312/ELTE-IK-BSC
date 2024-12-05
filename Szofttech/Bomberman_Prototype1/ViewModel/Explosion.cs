using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace Bomberman_Prototype1.ViewModel
{
    public class Explosion : ViewModelBase
    {
        public int X {  get; private set; }
        public int Y { get; private set; }

        private System.Timers.Timer timer = new System.Timers.Timer(500);

        public EventHandler<EventArgs>? ExplosionEnd;

        public Explosion(int x, int y)
        {
            X = x;
            Y = y;
            timer.Elapsed += ExplosionTimeoutEventHandler;
            timer.Start();
        }

        private void ExplosionTimeoutEventHandler(object? sender, EventArgs e)
        {
            timer.Stop();
            ExplosionEnd!.Invoke(this, new EventArgs());
        }
    }
}
