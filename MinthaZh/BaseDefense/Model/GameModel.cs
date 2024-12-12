using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Drawing;

namespace BaseDefense.Model
{
    public enum FieldType { EMPTY, BASE, SOLDIER, ENEMY}
    public class GameModel
    {
        public const int GAME_SIZE = 8;

        private FieldType[,]? GameTable;
        private Timer gameTimer;

        private List<Point> enemyList;
        private List<Point> soldierList;

        public int PointsForSoldier { get; private set; }
        public int BaseHp { get; private set; }

        public event EventHandler<FieldValueEventArgs>? FieldChanged;
        public event EventHandler<int>? BaseHpChanged;
        public event EventHandler? GameOver;
        public event EventHandler<int>? EnemyDied;

        public GameModel()
        {
            enemyList = new List<Point>();
            soldierList = new List<Point>();

            gameTimer = new Timer();
            gameTimer.AutoReset = true;
            gameTimer.Elapsed += GameTimer_Tick;
            gameTimer.Interval = 3000;
        }

        private void GameTimer_Tick(object? sender, ElapsedEventArgs e)
        {
            StepEnemies();
            SpawnEnemies();
            SoldierAction();
        }

        public void NewGame()
        {
            gameTimer.Stop();
            enemyList.Clear();
            soldierList.Clear();
            GameTable = new FieldType[GAME_SIZE, GAME_SIZE];
            for (int i = 0; i < GAME_SIZE; i++)
            {
                GameTable[0, i] = FieldType.BASE;
            }
            PointsForSoldier = 6;
            BaseHp = 20;
            BaseHpChanged?.Invoke(this, BaseHp);
            gameTimer.Start();
        }
        public void SaveGame()
        {

        }
        public void LoadGame()
        {

        }
        public void PlaceSoldier(int X, int Y)
        {
            if (GameTable![X, Y] == FieldType.EMPTY)
            {
                SetField(X, Y, FieldType.SOLDIER);
                soldierList.Add(new Point(X, Y));
            }
        }

        private void SetField(int x, int y, FieldType type)
        {
            GameTable![x,y] = type;
            FieldChanged?.Invoke(this, new FieldValueEventArgs(x,y,type));
        }
        private void SpawnEnemies()
        {
            Random r = new Random();
            int enemiesToSpawn = r.Next(1, 5); // 1-4 enemy
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                int rowToSpawn = r.Next(0, GAME_SIZE);
                SetField(GAME_SIZE - 1, rowToSpawn, FieldType.ENEMY);
                enemyList.Add(new Point(rowToSpawn, GAME_SIZE - 1));
            }
        }
        private void StepEnemies()
        {
            List<Point> toRemove = new List<Point>();
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (GameTable![enemyList[i].X, enemyList[i].Y - 1] == FieldType.EMPTY)
                {
                    SetField(enemyList[i].X, enemyList[i].Y, FieldType.EMPTY);
                    enemyList[i] = new Point(enemyList[i].X, enemyList[i].Y - 1);
                    SetField(enemyList[i].X, enemyList[i].Y, FieldType.ENEMY);
                } 
                else if(GameTable![enemyList[i].X, enemyList[i].Y - 1] == FieldType.SOLDIER)
                {
                    toRemove.Add(enemyList[i]);
                    KillSoldier(enemyList[i].X, enemyList[i].Y - 1);
                }
                else if (GameTable![enemyList[i].X, enemyList[i].Y - 1] == FieldType.BASE)
                {
                    BaseHp--;
                    BaseHpChanged?.Invoke(this, BaseHp);
                    if (BaseHp == 0)
                    {
                        gameTimer.Stop();
                        GameOver?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }
        private void KillSoldier(int X, int Y)
        {
            if (soldierList.Remove(new Point(X, Y)))
            {
                SetField(X, Y, FieldType.EMPTY);
            }
        }
        private void KillEnemy(int X, int Y)
        {
            if(enemyList.Remove(new Point(X, Y)))
            {
                SetField(X, Y, FieldType.EMPTY);
            }
        }
        private void SoldierAction()
        {
            foreach (Point soldier in soldierList)
            {
                if (GameTable![soldier.X - 1, soldier.Y] == FieldType.ENEMY)
                {
                    KillEnemy(soldier.X - 1, soldier.Y);
                }
                if (GameTable![soldier.X + 1, soldier.Y] == FieldType.ENEMY)
                {
                    KillEnemy(soldier.X + 1, soldier.Y);
                }
            }
        }
    }
}
