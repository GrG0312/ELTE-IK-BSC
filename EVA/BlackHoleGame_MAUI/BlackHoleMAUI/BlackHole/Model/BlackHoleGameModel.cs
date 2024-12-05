using BlackHole.Persistance;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHole.Model
{
    
    public class BlackHoleGameModel
    {
        #region Változók

        private IBlackHoleDataManager dataManager;
        private int shipsEnteredRed;
        private int shipsEnteredBlue;
        private bool isShipSelected;
        private BlackHoleTable table;
        private Point selectedShip;

        #endregion

        #region Property

        public int gameSize { get; private set; }
        public int currentParty { get; private set; }

        #endregion

        #region Eventek

        public event EventHandler<BlackHoleFieldEventArgs>? FieldHasChanged;
        public event EventHandler<int>? ShipEntered;
        public event EventHandler<int>? Victory;
        public event EventHandler? PartyChanged;
        public event EventHandler<BlackHoleLoadEventArgs>? TableLoaded;

        #endregion

        public BlackHoleGameModel(IBlackHoleDataManager dataManager)
        {
            this.dataManager = dataManager;
            table = new BlackHoleTable();
        }

        public void NewGame(int size)
        {
            table = new BlackHoleTable(size);
            shipsEnteredRed = 0;
            shipsEnteredBlue = 0;
            currentParty = 1;
            PartyChanged?.Invoke(this, EventArgs.Empty);
            isShipSelected = false;
            selectedShip = new Point(0,0);
            gameSize = size;

            //Populate Table
            int i = gameSize / 2;
            int offset = 0;
            do
            {
                i--;
                offset += 2;
                ChangeField(i, i, 1);
                ChangeField(i, i + offset, 2);
                ChangeField(i + offset, i, 1);
                ChangeField(i + offset, i + offset, 2);
            } while (i > 0);
            ChangeField(size / 2, size / 2, 3);
        }

        public void Click(int x, int y)
        {
            if (isShipSelected)//HA HAJÓ KIVÁLASZTVA
            {
                if (x == selectedShip.X)//OSZLOP STIMMEL
                {
                    if (y == selectedShip.Y-1 && (table.GetFieldValue(x,y) == 0 || table.GetFieldValue(x,y) == 3))//FELETTE
                    {
                        MoveShip(0);
                        return;
                    } else
                    if (y == selectedShip.Y+1 && (table.GetFieldValue(x, y) == 0 || table.GetFieldValue(x, y) == 3))//LEFELE
                    {
                        MoveShip(1);
                        return;
                    }
                } 
                else
                if (y == selectedShip.Y)//SOR STIMMEL
                {
                    if (x == selectedShip.X+1 && (table.GetFieldValue(x, y) == 0 || table.GetFieldValue(x, y) == 3))//JOBBRA
                    {
                        MoveShip(3);
                        return;
                    } else
                    if(x == selectedShip.X - 1 && (table.GetFieldValue(x, y) == 0 || table.GetFieldValue(x, y) == 3))//BALRA
                    {
                        MoveShip(2);
                        return;
                    }
                }
                ChangeField(selectedShip.X, selectedShip.Y, currentParty == 1 ? 4 : 5);
                isShipSelected = false;
            }
            if(!isShipSelected)//HA NINCS HAJÓ KIVÁLASZTVA
            {
                if (table.GetFieldValue(x,y) == currentParty)
                {
                    //select
                    isShipSelected = true;
                    selectedShip = new Point(x, y);
                    ChangeField(x, y, currentParty == 1 ? 4 : 5);
                    return;
                }
            }
        }

        public async Task SaveGameAsync(string path)
        {
            if (dataManager == null)
            {
                throw new InvalidDataException("No data access is provided!");
            }
            await dataManager.SaveGame(path, currentParty, shipsEnteredRed, shipsEnteredBlue, gameSize, table);
        }

        public async Task LoadGameAsync(string path)
        {
            if (dataManager == null)
                throw new InvalidOperationException("No data access is provided!");

            (int cp, int p1, int p2, int s, BlackHoleTable table) returned = await dataManager.LoadGame(path);

            currentParty = returned.cp;
            shipsEnteredRed = returned.p1;
            shipsEnteredBlue = returned.p2;
            gameSize = returned.s;
            //table = returned.table;
            TableLoaded?.Invoke(this, new BlackHoleLoadEventArgs(shipsEnteredRed, shipsEnteredBlue, gameSize));
            table = new BlackHoleTable(gameSize);
            for (int i = 0; i < gameSize; i++)
            {
                for (int j = 0; j < gameSize; j++)
                {
                    ChangeField(i, j, returned.table.GetFieldValue(i, j));
                }
            }
            
        }

        public int GetEnteredShipsNumber(int player)
        {
            if (player == 1)
            {
                return shipsEnteredRed;
            }
            else
            {
                return shipsEnteredBlue;
            }
        }

        public int GetTableFieldValue(int x, int y)
        {
            return table.GetFieldValue(x, y);
        }

        #region Privát metódusok
        private void ChangeField(int x, int y, int value)
        {
            if (value == 1 || value == 2 || value == 0 || value == 3)
            {
                table.SetFieldValue(x, y, value);
            }
            FieldHasChanged?.Invoke(this, new BlackHoleFieldEventArgs(x, y, value));
            /*
             * 0 - fehér
             * 1 - piros?
             * 2 - kék?
             * 3 - fekete
             * 4 - világos piros - azaz a piros hajók melletti kiválasztott mező
             * 5 - világos kék - a kék hajók melletti kiválaszott mező, ez és a piros csak vizuális, a gameModel táblázatában nem jelenik meg
             */
        }

        private void MoveShip(int direction)//0-fel 1-le 2-balra 3-jobbra
        {
            ChangeField(selectedShip.X, selectedShip.Y, currentParty == 1 ? 4 : 5);//leveszi a szelektálás kinézetét
            isShipSelected = false;//leveszi a szelektálást
            int i = 1;
            switch (direction)
            {
                case 0://felfele
                    while (table.GetFieldValue(selectedShip.X, selectedShip.Y - i) == 0)
                    {
                        i++;
                    }
                    if (table.GetFieldValue(selectedShip.X, selectedShip.Y - i) == 3)
                    {
                        if (currentParty == 1)
                        {
                            shipsEnteredRed++;
                        }
                        else
                        {
                            shipsEnteredBlue++;
                        }
                        ShipEntered?.Invoke(this, currentParty);
                        if (shipsEnteredRed == gameSize/2 || shipsEnteredBlue == gameSize/2)
                        {
                            Victory?.Invoke(this, currentParty);
                        }
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    else
                    {
                        ChangeField(selectedShip.X, selectedShip.Y - i + 1, currentParty);
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    break;
                case 1://lefele
                    while (table.GetFieldValue(selectedShip.X, selectedShip.Y + i) == 0)
                    {
                        i++;
                    }
                    if (table.GetFieldValue(selectedShip.X, selectedShip.Y + i) == 3)
                    {
                        if (currentParty == 1)
                        {
                            shipsEnteredRed++;
                        }
                        else
                        {
                            shipsEnteredBlue++;
                        }
                        ShipEntered?.Invoke(this, currentParty);
                        if (shipsEnteredRed == gameSize / 2 || shipsEnteredBlue == gameSize / 2)
                        {
                            Victory?.Invoke(this, currentParty);
                        }
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    else
                    {
                        ChangeField(selectedShip.X, selectedShip.Y + i - 1, currentParty);
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    break;
                case 2://balra
                    while (table.GetFieldValue(selectedShip.X - i, selectedShip.Y) == 0)
                    {
                        i++;
                    }
                    if (table.GetFieldValue(selectedShip.X - i, selectedShip.Y) == 3)
                    {
                        if (currentParty == 1)
                        {
                            shipsEnteredRed++;
                        }
                        else
                        {
                            shipsEnteredBlue++;
                        }
                        ShipEntered?.Invoke(this, currentParty);
                        if (shipsEnteredRed == gameSize / 2 || shipsEnteredBlue == gameSize / 2)
                        {
                            Victory?.Invoke(this, currentParty);
                        }
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    else
                    {
                        ChangeField(selectedShip.X - i + 1, selectedShip.Y, currentParty);
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    break;
                case 3://jobbra
                    while (table.GetFieldValue(selectedShip.X + i, selectedShip.Y) == 0)
                    {
                        i++;
                    }
                    if (table.GetFieldValue(selectedShip.X + i, selectedShip.Y) == 3)
                    {
                        if (currentParty == 1)
                        {
                            shipsEnteredRed++;
                        }
                        else
                        {
                            shipsEnteredBlue++;
                        }
                        ShipEntered?.Invoke(this, currentParty);
                        if (shipsEnteredRed == gameSize / 2 || shipsEnteredBlue == gameSize / 2)
                        {
                            Victory?.Invoke(this, currentParty);
                        }
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    else
                    {
                        ChangeField(selectedShip.X + i - 1, selectedShip.Y, currentParty);
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    break;
            }
            currentParty = currentParty == 1 ? 2 : 1;//átadja a kört '''effektíve'''
            PartyChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
