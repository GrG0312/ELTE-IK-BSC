using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Model
{
    public enum Field
    {
        EMPTY,
        MONSTER,
        BOMB,
        WALL,
        WALL_AND_MONSTER,
        WEAK_WALL,
        BOX,
        EFFECT,
        EXPLOSION,
        PLAYER
    }
}
