using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Model.Entities.Powerups
{
    public class Obstacle:EffectBase
    {
        private static readonly string URL = "../../../View/Resources/Effects/obstacle.png";
        public Obstacle(int x, int y) : base(x, y, URL) { }
    }
}
