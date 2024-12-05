using Bomberman_Prototype1.Model.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Model.Entities.Monsters
{
    public abstract class Monster : MovingEntity
    {
        private bool facade;
        // mozgás időzítője
        protected System.Timers.Timer movetimer;
        public abstract string Type { get; }
        // randomizer
        protected Random random;
        // fields it can go to
        protected abstract Field[] Allowed { get; }
        public event EventHandler<PlaceValueEventArgs<MovingEntity>>? MonsterMoved;
        public event EventHandler<PlaceValueEventArgs<Direction>>? GetFieldAhead;
        public Direction Direction { get; protected set; }
        //mi van előtte, checkeli majd lépés előtt
        public Field FieldAhead {  get; set; }
        public int MonsterID { get; private set; }
        public void Move(int col, int row)
        {
            Col=col; Row=row;
            OnMonsterMoved(this);
        }
        protected void OnMonsterMoved(object? sender)
        {
            MonsterMoved?.Invoke(sender, new PlaceValueEventArgs<MovingEntity>(col, row,this));
        }
        protected void OnGetFieldAhead(int row, int col, Direction dir)
        {
            GetFieldAhead?.Invoke(this, new PlaceValueEventArgs<Direction>(col, row, dir));
        }
        public Monster(int x, int y, string spritePath,int id, bool facade=false) : base(x, y, spritePath)
        {
            this.facade=facade;
            MonsterID= id;
            random= new Random();
            // timer init
            movetimer = new System.Timers.Timer(150);
            movetimer.Elapsed += OneMove;
            if (!facade)
            {
                movetimer.Start();
            }

            Direction = (Direction)(random.Next() % 4);  // random kezdeti irány
            FieldAhead = Field.EMPTY;//muszáj előtte üres helynek lennie!!
        }

        // általános mozgás eseménykezelő
        protected abstract void OneMove(object? sender, EventArgs e);


    }
}
