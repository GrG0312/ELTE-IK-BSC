using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Bomberman_Prototype1.Model.CustomEventArgs;

namespace Bomberman_Prototype1.Model.Entities
{
    public class Bomb : Entity
    {
        private static readonly string URL = "../../../View/Resources/Objects/bomb_2.png";
        private int fuseTime;
        /// <summary>
        /// Tells us if the bomb should detonate on its own (true), or not (false)
        /// </summary>
        private bool shouldDetonate;

        public int ExplosionRange { get; private set; }
        public System.Timers.Timer Timer { get; protected set; }

        public event EventHandler<PlaceValueEventArgs<Bomb>>? BombExploded;
        
        public Bomb(int x, int y, int range, bool detonator) : base(x, y, URL)
        {
            ExplosionRange = range;
            fuseTime = 3;
            shouldDetonate = !detonator;
            Timer = new System.Timers.Timer(1000);

            Timer.Elapsed += BombTicks;
            if (shouldDetonate)
            {
                Timer.Start();
            }
        }

        #region Public methods
        public void ExplodeNow()
        {
            Timer.Stop();
            BombExploded?.Invoke(this, new PlaceValueEventArgs<Bomb>(X, Y, this));
        }
        public void ChangeDetonationMode()
        {
            if (shouldDetonate)
            {
                Timer.Stop();
            } else
            {
                Timer.Start();
            }
            shouldDetonate = !shouldDetonate;
        }
        #endregion
        private void BombTicks(object? sender, EventArgs e)
        {
            --fuseTime;
            if (fuseTime <= 0)
            {
                ExplodeNow();
            }
        }
    }
}
