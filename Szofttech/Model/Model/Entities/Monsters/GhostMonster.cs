using Bomberman_Prototype1.Model.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Model.Entities.Monsters
{
    public class GhostMonster : Monster
    {
        private bool inAWall = false;

        // eltárolja, hogy akadályban van-e
        public override string Type { get { return "ghost"; } }

        public bool CanMove { get; set; }

        protected override Field[] Allowed
        {
            get
            {
                return [Field.EMPTY, Field.WEAK_WALL, Field.WALL, Field.BOX];
            }
        }

        public event EventHandler<PlaceValueEventArgs<Direction>>? CheckAheadEvent;

        public GhostMonster(int x, int y, string spritePath, int id, bool facade = false) : base(x, y, spritePath, id, facade)
        {
            CanMove = false;
            movetimer.Interval = 350;
        }

        /// <summary>
        /// moves according to the monster's behaviour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OneMove(object? sender, EventArgs e)
        {
            CheckAheadEvent?.Invoke(this, new PlaceValueEventArgs<Direction>(Col, Row, Direction));
            if (Allowed.Contains(FieldAhead) && CanMove)
            {
                MoveAhead();
            }
            else
            {
                if (!LookForDirection())
                {
                    return;
                }
            }

            OnMonsterMoved(this);

            // alapból 1/10 esély random irányváltoztatásra
            if (random.Next() % 10 == 9 && !inAWall)
            {
                Direction[] otherDirs = [Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT];
                otherDirs = otherDirs.Except([Direction]).ToArray();
                RandomDirectionSet(otherDirs);
            }
        }

        private void MoveAhead()
        {
            switch (Direction)
            {
                case Direction.UP:
                    Row--;
                    break;
                case Direction.DOWN:
                    Row++;
                    break;
                case Direction.LEFT:
                    Col--;
                    break;
                case Direction.RIGHT:
                    Col++;
                    break;
            }
        }

        private void RandomDirectionSet(Direction[] directions)
        {
            Direction = directions[random.Next() % directions.Length];
        }

        // assumes current direction doesn't work
        private bool LookForDirection()
        {
            // ez egy csúnya megvalósítás
            Direction[] tocheck = [Direction.UP, Direction.LEFT, Direction.DOWN, Direction.RIGHT];
            tocheck = tocheck.Except([Direction]).ToArray();
            RandomDirectionSet(tocheck);    // 3 irány
            CheckAheadEvent?.Invoke(this, new PlaceValueEventArgs<Direction>(Col, Row, Direction));
            if (Allowed.Contains(FieldAhead) && CanMove ) { return true; }
            tocheck = tocheck.Except([Direction]).ToArray();
            RandomDirectionSet(tocheck);    // 2 irány
            CheckAheadEvent?.Invoke(this, new PlaceValueEventArgs<Direction>(Col, Row, Direction));
            if (Allowed.Contains(FieldAhead) && CanMove) { return true; }
            tocheck = tocheck.Except([Direction]).ToArray();
            RandomDirectionSet(tocheck);    // 1 irány
            CheckAheadEvent?.Invoke(this, new PlaceValueEventArgs<Direction>(Col, Row, Direction));
            if (Allowed.Contains(FieldAhead) && CanMove) { return true; }

            return false;
        }
    }
}
