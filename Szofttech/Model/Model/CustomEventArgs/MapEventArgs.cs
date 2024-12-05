using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman_Prototype1.Model.Entities;
using Bomberman_Prototype1.Model.Entities.Monsters;

namespace Bomberman_Prototype1.Model.CustomEventArgs
{
    public class MapEventArgs : EventArgs
    {
        private Field[,] _map;
        private int _rows;
        private int _cols;
        private int _type;
        public Field[,] Map { get { return _map; } }
        public int Rows { get { return _rows; } }
        public int Cols { get { return _cols; } }
        public int Type { get { return _type; } }
        public Player[] Players { get; private set; }
        public Monster[] Monsters { get; private set; }
        public MapEventArgs(Field[,] map, int cols, int rows, int type, List<Player> plrs, List<Monster> monsters)
        {
            _map = map;
            _rows = rows;
            _cols = cols;
            _type = type;

            Players = new Player[plrs.Count];
            for (int i = 0; i < plrs.Count; i++)
            {
                Players[i] = plrs[i];
            }
            Monsters =new Monster[monsters.Count];
            for (int i = 0; i < monsters.Count; ++i)
            {
                Monsters[i] = monsters[i];
            }
        }
    }
}
