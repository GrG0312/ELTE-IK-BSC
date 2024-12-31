using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace BaseDefense.Model
{
    public enum FieldType { EMPTY, BASE, SOLDIER, ENEMY}
    public class GameModel
    {
        private bool isGameOver;

        public const int GAME_SIZE = 8;
        public const int ENEMY_DMG = 5;

        private FieldType[,]? GameTable;
        private Timer gameTimer;

        private List<Point> enemyList;
        private List<Point> soldierList;

        public int PointsForSoldier { get; private set; }
        public int BaseHp { get; private set; }

        public event EventHandler<FieldValueEventArgs>? FieldChanged;
        public event EventHandler? BaseHpChanged;
        public event EventHandler? GameOver;
        public event EventHandler? PointsForSoldierChanged;

        public GameModel()
        {
            enemyList = new List<Point>();
            soldierList = new List<Point>();

            gameTimer = new Timer();
            gameTimer.AutoReset = true;
            gameTimer.Elapsed += GameTimer_Tick;
            gameTimer.Interval = 2000;
        }
        #region Publikus metódusok
        public void NewGame()
        {
            gameTimer.Stop();
            enemyList.Clear();
            soldierList.Clear();
            isGameOver = false;
            GameTable = new FieldType[GAME_SIZE, GAME_SIZE];
            for (int i = 0; i < GAME_SIZE; i++)
            {
                SetField(i, 0, FieldType.BASE); // x melyik sorba vagy, y melyik oszlopban
            }
            ResetBaseAndPoints();
            gameTimer.Start();
        }

        #region Mentés-Betöltés
        public async void SaveGame(string path)
        {
            // átlagos mentési folyamat
            // dobjunk exception-t, ha nem tudunk menteni valami miatt!
        }
        public async void LoadGame(string path)
        {
            // átlagos betöltési folyamat
            // dobjunk exception-t, ha nem tudjuk betölteni valami miatt!
        }
        #endregion

        public void Click(int X, int Y)
        {
            if (isGameOver)
            {
                return;
            }
            if (GameTable![X, Y] == FieldType.EMPTY && PointsForSoldier >= 3)
            {
                SetField(X, Y, FieldType.SOLDIER);
                soldierList.Add(new Point(X, Y));
                AddPointsForSoldier(-3);
            }
        }
        #endregion

        #region Privát metódusok

        #region Set metódusok
        private void SetField(int x, int y, FieldType type)
        {
            GameTable![x,y] = type;
            FieldChanged?.Invoke(this, new FieldValueEventArgs(x,y,type));
        }
        private void ResetBaseAndPoints()
        {
            BaseHp = 100;
            BaseHpChanged?.Invoke(this, EventArgs.Empty);
            PointsForSoldier = 6;
            PointsForSoldierChanged?.Invoke(this, EventArgs.Empty);
        }
        private void AddPointsForSoldier(int plus = 1)
        {
            PointsForSoldier += plus;
            PointsForSoldierChanged?.Invoke(this, EventArgs.Empty);
        }
        private void DmgBase()
        {
            BaseHp -= ENEMY_DMG;
            BaseHpChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
        private void SpawnEnemies()
        {
            Random r = new Random();
            int enemiesToSpawn = r.Next(1, 4); // 1-3 enemy
            List<Point> occupied = new List<Point>();
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                int rowToSpawn;
                do
                {
                    rowToSpawn = r.Next(0, GAME_SIZE);
                } while (occupied.Contains(new Point(rowToSpawn, GAME_SIZE - 1)));
                SetField(rowToSpawn, GAME_SIZE - 1, FieldType.ENEMY);
                enemyList.Add(new Point(rowToSpawn, GAME_SIZE - 1));
            }
        }

        #region Enemy és Soldier metódusok
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
                    toRemove.Add(enemyList[i]);
                    DmgBase();
                    if (BaseHp <= 0)
                    {
                        isGameOver = true;
                        gameTimer.Stop();
                        GameOver?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            foreach (Point enemy in toRemove)
            {
                KillEnemy(enemy.X, enemy.Y);
            }
        }
        private void SoldierAction()
        {
            foreach (Point soldier in soldierList)
            {
                try
                {
                    if (GameTable![soldier.X - 1, soldier.Y] == FieldType.ENEMY)
                    {
                        KillEnemy(soldier.X - 1, soldier.Y);
                    }
                } catch (IndexOutOfRangeException) { /*Index out of bounds*/}
                try
                {
                    if (GameTable![soldier.X + 1, soldier.Y] == FieldType.ENEMY)
                    {
                        KillEnemy(soldier.X + 1, soldier.Y);
                    }
                }
                catch (IndexOutOfRangeException) { /*Index out of bounds*/ }
            }
        }
        #endregion

        #region Kill metódusok
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
                AddPointsForSoldier();
            }
        }
        #endregion
        private void GameTimer_Tick(object? sender, ElapsedEventArgs e)
        {
            StepEnemies();
            SpawnEnemies();
            SoldierAction();
        }
        #endregion
    }
}
