using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Model.Entities.Monsters
{
    public class BasicMonster : Monster
    {
        public override string Type { get { return "basic"; } }
        protected override Field[] Allowed
        {
            get
            {
                return [Field.EMPTY];
            }
        }

        public BasicMonster(int x, int y, string spritePath,int id, bool facade=false) : base(x, y, spritePath,id,facade) { }

        /// <summary>
        /// moves according to the monster's behaviour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OneMove(object? sender, EventArgs e)
        {
            if(X==70*col-15 && Y == row * 70)
            {
                OnGetFieldAhead(row, col, Direction);
                if (!Allowed.Contains(FieldAhead))
                {
                    if (!LookForDirection())
                    {
                        return;
                    }
                }

                MoveAhead();

                // alapból 1/10 esély random irányváltoztatásra
                if (random.Next() % 10 == 9)
                {
                    Direction[] otherDirs = [Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT];
                    otherDirs = otherDirs.Except([Direction]).ToArray();
                    RandomDirectionSet(otherDirs);
                }
                OnMonsterMoved(this);
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
            OnGetFieldAhead(row, col, Direction);
            if(Allowed.Contains(FieldAhead)) { return true; }
            tocheck = tocheck.Except([Direction]).ToArray();
            RandomDirectionSet(tocheck);    // 2 irány
            OnGetFieldAhead(row, col, Direction);
            if (Allowed.Contains(FieldAhead)) { return true; }
            tocheck = tocheck.Except([Direction]).ToArray();
            RandomDirectionSet(tocheck);    // 1 irány
            OnGetFieldAhead(row, col, Direction);
            if (Allowed.Contains(FieldAhead)) { return true; }

            return false;
        }
    }
}
