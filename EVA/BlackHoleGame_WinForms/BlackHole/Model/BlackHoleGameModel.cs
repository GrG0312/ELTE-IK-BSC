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
    public enum Player { RED = 1, BLUE = 2 }
    public class BlackHoleGameModel
    {
        #region Változók

        private IDataAccess dataManager;
        private int shipsEnteredRed;
        private int shipsEnteredBlue;
        private bool isShipSelected;
        private BlackHoleTable? table;
        private Point selectedShip;
        private Dictionary<Player, int> enteredShips;
        #endregion

        #region Property
        public int GameSize { get; private set; }
        public Player CurrentPlayer { get; private set; }
        #endregion

        #region Operators
        public int this[Player player]
        {
            get => enteredShips[player];
        }
        #endregion

        #region Eventek
        /// <summary>
        /// Triggers when a change has occured in one of the table's fields
        /// </summary>
        public event EventHandler<PlaceValueEventArgs>? FieldHasChanged;
        /// <summary>
        /// Triggers when a ship stepped on the central field, aka. the black hole
        /// </summary>
        public event EventHandler<Player>? ShipEntered;
        /// <summary>
        /// Triggers when one of the players has managed to get half of their ships into the black hole
        /// </summary>
        public event EventHandler<Player>? Victory;
        /// <summary>
        /// Triggers after a player moved it's ship
        /// </summary>
        public event EventHandler? PartyChanged;
        /// <summary>
        /// Triggers when the load method has finished
        /// </summary>
        public event EventHandler<PlaceValueEventArgs>? TableLoaded;
        #endregion

        public BlackHoleGameModel(IDataAccess dataManager)
        {
            enteredShips = new Dictionary<Player, int>()
            {
                { Player.RED, 0 },
                { Player.BLUE, 1 },
            };
            this.dataManager = dataManager;
        }

        public void NewGame(int size)
        {
            table = new BlackHoleTable(size);
            shipsEnteredRed = 0;
            shipsEnteredBlue = 0;
            CurrentPlayer = (Player)1;
            PartyChanged?.Invoke(this, EventArgs.Empty);
            isShipSelected = false;
            selectedShip = new Point(0,0);
            GameSize = size;

            //Populate Table
            int i = GameSize / 2;
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
        /// <summary>
        /// Simulates a click on the grid. Based on wether a ship is selected or not, it deselects
        /// </summary>
        /// <param name="x">Clicked Column</param>
        /// <param name="y">Clicked Row</param>
        public void Click(int x, int y)
        {
            // If ship is selected ...
            if (isShipSelected)
            {
                if (x == selectedShip.X) // ... And the Clicked Column is the same as the Selected ...
                {
                    if (y == selectedShip.Y-1 && (table!.GetFieldValue(x,y) == 0 || table.GetFieldValue(x,y) == 3))//FELETTE
                    {
                        MoveShip(0);
                        return;
                    } else
                    if (y == selectedShip.Y+1 && (table!.GetFieldValue(x, y) == 0 || table.GetFieldValue(x, y) == 3))//LEFELE
                    {
                        MoveShip(1);
                        return;
                    }
                } 
                else if (y == selectedShip.Y) // ... And the Clicked Row is the same as the Selected ...
                {
                    if (x == selectedShip.X+1 && (table!.GetFieldValue(x, y) == 0 || table.GetFieldValue(x, y) == 3))//JOBBRA
                    {
                        MoveShip(3);
                        return;
                    } else
                    if(x == selectedShip.X - 1 && (table!.GetFieldValue(x, y) == 0 || table.GetFieldValue(x, y) == 3))//BALRA
                    {
                        MoveShip(2);
                        return;
                    }
                }
                ChangeField(selectedShip.X, selectedShip.Y, CurrentPlayer == Player.RED ? 4 : 5);
                isShipSelected = false;
            }
            // If no ship is selected ...
            else
            {
                // ... And the clicked field is a ship of our own ...
                if ((Player)table!.GetFieldValue(x,y) == CurrentPlayer)
                {
                    // ... then we shall select it
                    isShipSelected = true;
                    selectedShip = new Point(x, y);
                    ChangeField(x, y, CurrentPlayer == Player.RED ? 4 : 5);
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
            await dataManager.SaveGame(path, (int)CurrentPlayer, shipsEnteredRed, shipsEnteredBlue, GameSize, table!);
        }

        public async Task LoadGameAsync(string path)
        {
            if (dataManager == null)
                throw new InvalidOperationException("No data access is provided!");

            (int cp, int p1, int p2, int s, BlackHoleTable table) returned = await dataManager.LoadGame(path);

            CurrentPlayer = (Player)returned.cp;
            shipsEnteredRed = returned.p1;
            shipsEnteredBlue = returned.p2;
            GameSize = returned.s;
            TableLoaded?.Invoke(this, new PlaceValueEventArgs(shipsEnteredRed, shipsEnteredBlue, GameSize));
            table = new BlackHoleTable(GameSize);
            for (int i = 0; i < GameSize; i++)
            {
                for (int j = 0; j < GameSize; j++)
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
            return table!.GetFieldValue(x, y);
        }

        #region Privát metódusok
        private void ChangeField(int x, int y, int value)
        {
            if (value == 1 || value == 2 || value == 0 || value == 3)
            {
                table!.SetFieldValue(x, y, value);
            }
            FieldHasChanged?.Invoke(this, new PlaceValueEventArgs(x, y, value));
            /*
             * 0 - fehér
             * 1 - piros
             * 2 - kék
             * 3 - fekete
             * 4 - világos piros
             * 5 - világos kék
             */
        }

        private void MoveShip(int direction)//0-fel 1-le 2-balra 3-jobbra
        {
            ChangeField(selectedShip.X, selectedShip.Y, CurrentPlayer == Player.RED ? 4 : 5);//leveszi a szelektálás kinézetét
            isShipSelected = false;//leveszi a szelektálást
            int i = 1;
            switch (direction)
            {
                case 0://felfele
                    while (table!.GetFieldValue(selectedShip.X, selectedShip.Y - i) == 0)
                    {
                        i++;
                    }
                    if (table.GetFieldValue(selectedShip.X, selectedShip.Y - i) == 3)
                    {
                        if (CurrentPlayer == Player.RED)
                        {
                            shipsEnteredRed++;
                        }
                        else
                        {
                            shipsEnteredBlue++;
                        }
                        ShipEntered?.Invoke(this, CurrentPlayer);
                        if (shipsEnteredRed == GameSize/2 || shipsEnteredBlue == GameSize/2)
                        {
                            Victory?.Invoke(this, CurrentPlayer);
                        }
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    else
                    {
                        ChangeField(selectedShip.X, selectedShip.Y - i + 1, (int)CurrentPlayer);
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    break;
                case 1://lefele
                    while (table!.GetFieldValue(selectedShip.X, selectedShip.Y + i) == 0)
                    {
                        i++;
                    }
                    if (table.GetFieldValue(selectedShip.X, selectedShip.Y + i) == 3)
                    {
                        if (CurrentPlayer == Player.RED)
                        {
                            shipsEnteredRed++;
                        }
                        else
                        {
                            shipsEnteredBlue++;
                        }
                        ShipEntered?.Invoke(this, CurrentPlayer);
                        if (shipsEnteredRed == GameSize / 2 || shipsEnteredBlue == GameSize / 2)
                        {
                            Victory?.Invoke(this, CurrentPlayer);
                        }
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    else
                    {
                        ChangeField(selectedShip.X, selectedShip.Y + i - 1, (int)CurrentPlayer);
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    break;
                case 2://balra
                    while (table!.GetFieldValue(selectedShip.X - i, selectedShip.Y) == 0)
                    {
                        i++;
                    }
                    if (table.GetFieldValue(selectedShip.X - i, selectedShip.Y) == 3)
                    {
                        if (CurrentPlayer == Player.RED)
                        {
                            shipsEnteredRed++;
                        }
                        else
                        {
                            shipsEnteredBlue++;
                        }
                        ShipEntered?.Invoke(this, CurrentPlayer);
                        if (shipsEnteredRed == GameSize / 2 || shipsEnteredBlue == GameSize / 2)
                        {
                            Victory?.Invoke(this, CurrentPlayer);
                        }
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    else
                    {
                        ChangeField(selectedShip.X - i + 1, selectedShip.Y, (int)CurrentPlayer);
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    break;
                case 3://jobbra
                    while (table!.GetFieldValue(selectedShip.X + i, selectedShip.Y) == 0)
                    {
                        i++;
                    }
                    if (table.GetFieldValue(selectedShip.X + i, selectedShip.Y) == 3)
                    {
                        if (CurrentPlayer == Player.RED)
                        {
                            shipsEnteredRed++;
                        }
                        else
                        {
                            shipsEnteredBlue++;
                        }
                        ShipEntered?.Invoke(this, CurrentPlayer);
                        if (shipsEnteredRed == GameSize / 2 || shipsEnteredBlue == GameSize / 2)
                        {
                            Victory?.Invoke(this, CurrentPlayer);
                        }
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    else
                    {
                        ChangeField(selectedShip.X + i - 1, selectedShip.Y, (int)CurrentPlayer);
                        ChangeField(selectedShip.X, selectedShip.Y, 0);
                    }
                    break;
            }
            CurrentPlayer = (Player)(CurrentPlayer == Player.RED ? 2 : 1);//átadja a kört '''effektíve'''
            PartyChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
