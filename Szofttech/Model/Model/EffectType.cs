using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Model
{
    public enum EffectType
    {
        SpeedUp,        //Permanent, Not stackable
        SlowDown,       //Time based (not permanent)
        Detonator,      //After placing all bombs, the player can detonate them manually
        Invincibility,  //Time based, visual effects
        Ghost,          //Time based, visual effects
        Obstacle,       //Stackable, first effect -> 3 obstacles, second effect -> 6 obstacle ...
        BiggerRange,    //+1 bomb explosion range
        OneRange,       //Time based, Bomb explosion range is fixed at 1
        PlusBomb,       //Permanent, +1 placable bomb
        NoBombs,        //Time based, can't place bombs while it lasts
        InstaPlaceBombs //Time based, if possible, instantly places bombs under the player
    };
}
