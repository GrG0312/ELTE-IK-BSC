using BlackHole.Model;
using BlackHole.Persistance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackHole.WinForms.View
{
    public partial class GameForm : Form
    {

        private IDataAccess dataManager;
        private BlackHoleGameModel gameModel;
        private Button[,] grid;
        private int sizeCoefficient;

        public GameForm()
        {
            InitializeComponent();

            //ablakméret beállítása
            Height = Convert.ToInt32(Math.Round(Screen.GetBounds(this).Height * 0.75));
            Width = Height;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;


            dataManager = new BlackHoleDataHandler();
            gameModel = new BlackHoleGameModel(dataManager);
            grid = new Button[0, 0];

            gameModel.FieldHasChanged += new EventHandler<PlaceValueEventArgs>(Model_OnFieldChange);
            gameModel.ShipEntered += new EventHandler<Player>(Model_ShipHasEntered);
            gameModel.Victory += new EventHandler<Player>(Model_Victory);
            gameModel.TableLoaded += new EventHandler<PlaceValueEventArgs>(Model_Load);

            GenerateTable(5);
            gameModel.NewGame(5);
            saveGameMenuItem.Enabled = true;

            MessageBox.Show("First step goes to 'Red'!", "The game has started", MessageBoxButtons.OK);
        }

        #region Metódusok
        private void GenerateTable(int size)
        {
            if (grid != null)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        Controls.Remove(grid[j, i]);
                    }
                }
            }
            sizeCoefficient = Width / (size + 2);
            grid = new Button[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = new Button();
                    grid[i, j].Size = new Size(sizeCoefficient, sizeCoefficient);
                    grid[i, j].Location = new Point(sizeCoefficient + sizeCoefficient * i, sizeCoefficient + sizeCoefficient * j);
                    grid[i, j].FlatStyle = FlatStyle.Flat;
                    grid[i, j].TabIndex = 100 * i + j;
                    grid[i, j].BackColor = Color.White;
                    grid[i, j].MouseClick += ButtonGrid_MouseClick;

                    Controls.Add(grid[i, j]);
                }
            }
        }

        private void ButtonGrid_MouseClick(object? sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                int x = button.TabIndex / 100;
                int y = button.TabIndex % 100;

                gameModel.Click(x, y);
            }
        }

        #region Új játék metódusok
        private void smallNewGameMenuItem_Click(object sender, EventArgs e)
        {
            GenerateTable(5);
            gameModel.NewGame(5);
            saveGameMenuItem.Enabled = true;
        }

        private void mediumNewGameMenuItem_Click(object sender, EventArgs e)
        {
            GenerateTable(7);
            gameModel.NewGame(7);
            saveGameMenuItem.Enabled = true;
        }

        private void largeNewGameMenuItem_Click(object sender, EventArgs e)
        {
            GenerateTable(9);
            gameModel.NewGame(9);
            saveGameMenuItem.Enabled = true;
        }
        private async void saveGameMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await gameModel.SaveGameAsync(saveFileDialog.FileName);
                }
                catch (BlackHoleDataException)
                {
                    MessageBox.Show("Játék mentése sikertelen" + Environment.NewLine
                        + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private async void loadGameMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék betöltése
                    await gameModel.LoadGameAsync(openFileDialog.FileName);
                }
                catch (BlackHoleDataException)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine
                        + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                saveGameMenuItem.Enabled = true;
            }
        }
        #endregion

        #region Event Handler methods
        private void Model_Load(object? sender, PlaceValueEventArgs e)
        {
            player1shipsNumber.Text = e.X.ToString();
            player2shipsNumber.Text = e.Y.ToString();
            GenerateTable(e.Value);
        }
        private void Model_Victory(object? sender, Player winner)
        {
            switch (winner)
            {
                case Player.RED:
                    MessageBox.Show("Congratulations! Player 'Red' has won the game!", "Game Over", MessageBoxButtons.OK);
                    break;
                case Player.BLUE:
                    MessageBox.Show("Congratulations! Player 'Blue' has won the game!", "Game Over", MessageBoxButtons.OK);
                    break;
                default:
                    throw new ArgumentException($"Invalid player enum value: {winner}");
            }

            saveGameMenuItem.Enabled = false;

            //Disable all buttons
            for (int i = 0; i < gameModel.GameSize; i++)
            {
                for (int j = 0; j < gameModel.GameSize; j++)
                {
                    grid[i, j].Enabled = false;
                }
            }
        }
        private void Model_OnFieldChange(object? sender, PlaceValueEventArgs e)
        {
            switch (e.Value)
            {
                case 0:// NEUTRAL ZONE
                    grid[e.X, e.Y].BackColor = Color.White;
                    break;

                case 1:// PLAYER.RED SHIP
                    grid[e.X, e.Y].BackColor = Color.Red;
                    break;

                case 2:// PLAYER.BLUE SHIP
                    grid[e.X, e.Y].BackColor = Color.Blue;
                    break;

                case 3:// BLACKHOLE
                    grid[e.X, e.Y].BackColor = Color.Black;
                    break;

                case 4://PLAYER.RED SELECT/DESELECT
                    // Try-Catch blocks are needed to ensure that the program doesn't **DIE** from IndexOutOfRange
                    // Separate blocks are needed to ensure that all 4 are executed
                    // (well at least try to get executed)
                    try
                    {
                        grid[e.X, e.Y - 1].BackColor = DetermineCorrectColor(grid[e.X, e.Y - 1].BackColor, Color.Crimson);
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        grid[e.X, e.Y + 1].BackColor = DetermineCorrectColor(grid[e.X, e.Y + 1].BackColor, Color.Crimson);
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        grid[e.X + 1, e.Y].BackColor = DetermineCorrectColor(grid[e.X + 1, e.Y].BackColor, Color.Crimson);
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        grid[e.X - 1, e.Y].BackColor = DetermineCorrectColor(grid[e.X - 1, e.Y].BackColor, Color.Crimson);
                    }
                    catch (IndexOutOfRangeException) { }
                    break;
                case 5: // PLAYER.BLUE SELECT/DESELECT
                    try
                    {
                        grid[e.X, e.Y - 1].BackColor = DetermineCorrectColor(grid[e.X, e.Y - 1].BackColor, Color.Cyan);
                    }
                    catch (Exception) { }
                    try
                    {
                        grid[e.X, e.Y + 1].BackColor = DetermineCorrectColor(grid[e.X, e.Y + 1].BackColor, Color.Cyan);
                    }
                    catch (Exception) { }
                    try
                    {
                        grid[e.X + 1, e.Y].BackColor = DetermineCorrectColor(grid[e.X + 1, e.Y].BackColor, Color.Cyan);
                    }
                    catch (Exception) { }
                    try
                    {
                        grid[e.X - 1, e.Y].BackColor = DetermineCorrectColor(grid[e.X - 1, e.Y].BackColor, Color.Cyan);
                    }
                    catch (Exception) { }
                    break;
            }
        }
        private void Model_ShipHasEntered(object? sender, Player player)
        {
            string newText = gameModel[player].ToString();
            switch (player)
            {
                case Player.RED:
                    player1shipsNumber.Text = newText;
                    break;
                case Player.BLUE:
                    player2shipsNumber.Text = newText;
                    break;
                default:
                    throw new ArgumentException($"Invalid player enum value: {player}");
            }
        }
        private Color DetermineCorrectColor(Color set, Color selected)
        {
            //return set == Color.White ? selected : Color.White;
            if (set == Color.White)
            {
                return selected;
            }
            else
            if (set == selected)
            {
                return Color.White;
            }
            return set;
        }
        #endregion

        #endregion

    }
}
