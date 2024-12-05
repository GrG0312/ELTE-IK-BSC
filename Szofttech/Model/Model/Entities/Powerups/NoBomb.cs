using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Model.Entities.Powerups
{
    public class NoBomb : EffectBase
    {
        private static readonly string URL = "../../../View/Resources/Effects/nobomb.png";

        public NoBomb(int x, int y) : base(x,y,URL) { }
    }
}
