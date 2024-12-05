using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BlackHole.Model;

namespace BlackHole.Persistance
{
    public class BlackHoleTable
    {
        private int[,] gameObjectTable;//0--semmi 1--player1(red) 2--player2(blue) 3--black hole

        #region Constructors
        public BlackHoleTable() : this(5) { }

        public BlackHoleTable(int gameSize)
        {
            gameObjectTable = new int[gameSize, gameSize];//mind nulla
        }

        #endregion

        #region Publikus metódusok
        public int GetFieldValue(int x, int y)
        {
            try
            {
                return gameObjectTable[x, y];
            } catch (Exception)
            {
                return 99;
            }
        }

        public void SetFieldValue(int x, int y, int value)
        {
            gameObjectTable[x, y] = value;
        }

        #endregion
    }
}
