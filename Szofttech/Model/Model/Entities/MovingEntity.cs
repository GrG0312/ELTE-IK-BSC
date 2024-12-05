using Bomberman_Prototype1.Model.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Model.Entities
{
    public abstract class MovingEntity : Entity
    {
        public override int Row
        {
            get { return row; }
            protected set
            {
                if (row == 0)
                {
                    row = value;
                } else
                if (row != value && !timerCol.Enabled)
                {
                    row = value;
                    targetY = value * 70;
                    if (targetY != Y)
                        timerRow.Start();
                    OnPropertyChanged(nameof(Y));
                }
            }
        }
        public override int Col
        {
            get
            {
                return col;
            }
            protected set
            {
                if (col == 0)
                {
                    col = value;
                } else
                if (col != value && !timerRow.Enabled)
                {
                    col = value;
                    targetX = value * 70 - 15;
                    if (targetX != X)
                        timerCol.Start();
                    OnPropertyChanged(nameof(X));
                }
            }
        }

        private int targetX, targetY;
        private System.Timers.Timer timerCol;
        private System.Timers.Timer timerRow;
        protected int moveSpeed { get; private set; }

        public MovingEntity(int col, int row, string url) : base(col, row, url)
        {
            timerRow = new System.Timers.Timer(20);
            timerRow.Elapsed += TimerTickRow;
            timerCol = new System.Timers.Timer(20);
            timerCol.Elapsed += TimerTickCol;

            targetX = X;
            targetY = Y;
            moveSpeed = 7;
        }

        //speed megváltoztatásához
        protected void ChangeSpeed(int newSpeed) 
        { 
            moveSpeed = newSpeed;
        }
        private void TimerTickRow(object? sender, EventArgs e)
        {
            if (Y != targetY)
            {
                if (Y < targetY)
                {
                    if (targetY - Y < moveSpeed)
                    {
                        Y = targetY;
                    }
                    else
                    {
                        Y += moveSpeed;
                    }
                }
                else
                {
                    if (Y - targetY < moveSpeed)
                    {
                        Y = targetY;
                    }
                    else
                    {
                        Y -= moveSpeed;
                    }
                }
            }
            else
            {
                timerRow.Stop();
            }
        }
        private void TimerTickCol(object? sender, EventArgs e)
        {
            if (X != targetX)
            {
                if (X < targetX)
                {
                    if (targetX - X < moveSpeed)
                    {
                        X = targetX;
                    }
                    else
                    {
                        X += moveSpeed;
                    }
                }
                else
                {
                    if (X - targetX < moveSpeed)
                    {
                        X = targetX;
                    }
                    else
                    {
                        X -= moveSpeed;
                    }
                }
            }
            else
            {
                timerCol.Stop();
            }
        }
    }
}
