using Bomberman_Prototype1.Model.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Model.Entities.Monsters
{
    public class SmartMonster : Monster
    {
        public override string Type { get { return "smart"; } }

        public Direction BestDir { get; set; }

        public bool SeesPlayer { get; set; }

        protected override Field[] Allowed
        {
            get
            {
                return [Field.EMPTY];
            }
        }

        public event EventHandler<PlaceEventArgs>? GetBestDirEvent;

        public SmartMonster(int x, int y, string spritePath, int id, bool facade = false) : base(x, y, spritePath, id, facade)
        {
            movetimer.Interval = 450;
            BestDir = Direction;
            SeesPlayer = false;
        }

        /// <summary>
        /// moves according to the monster's behaviour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OneMove(object? sender, EventArgs e)
        {
            GetBestDirEvent?.Invoke(this, new PlaceEventArgs(Col, Row));
            if (SeesPlayer)
            {
                Direction = BestDir;
                if (random.Next() % 7 == 6)
                {
                    LookForDirection();
                }
            }
            else if (!LookForDirection())
            {
                return;
            }

            MoveAhead();
            OnMonsterMoved(this);
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
            if (Allowed.Contains(FieldAhead)) { return true; }
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
