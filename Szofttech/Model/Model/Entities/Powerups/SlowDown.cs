using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman_Prototype1.Model.CustomEventArgs;

namespace Bomberman_Prototype1.Model.Entities.Powerups
{
    public class SlowDown : EffectBase
    {
        private static readonly string URL = "../../../View/Resources/Effects/slowdown.png";

        public int Duration { get; private set; }
        public SlowDown(int x, int y) : base(x, y, URL) { }
    }
}
