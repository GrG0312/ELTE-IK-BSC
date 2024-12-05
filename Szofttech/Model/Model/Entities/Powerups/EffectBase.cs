using Bomberman_Prototype1.Model.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Model.Entities.Powerups
{
    public abstract class EffectBase : Entity
    {
        public EffectBase(int col, int row, string url) : base(col, row, url) { X += 15; }
        //X+=15: Mivel bal felső saroktól számolja a pozit, alrébb kell tenni kicsit, h középen legyen
    }
}
