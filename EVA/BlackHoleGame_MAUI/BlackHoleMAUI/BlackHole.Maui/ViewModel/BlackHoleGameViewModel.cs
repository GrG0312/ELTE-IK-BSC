using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BlackHole.Model;

namespace BlackHole.ViewModel
{
    public class BlackHoleGameViewModel : ViewModelBase
    {
        private BlackHoleGameModel gameModel;
        private int size;

        #region Properties
        public ObservableCollection<BlackHoleField> Fields { get; set; }
        public DelegateCommand IWantNewGame { get; private set; }
        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public DelegateCommand ExitGameCommand { get; private set; }
        public Color IsActivePlayerRed { get { return gameModel.currentParty == 1 ? Colors.Red : Colors.Black; } }
        public Color IsActivePlayerBlue { get { return gameModel.currentParty == 2 ? Colors.Blue : Colors.Black; } }
        public string RedShipsNumber { get { return gameModel.GetEnteredShipsNumber(1).ToString(); } }
        public string BlueShipsNumber { get { return gameModel.GetEnteredShipsNumber(2).ToString(); } }
        public bool IsSaveOkay { get; private set; } = true;

        //PLUS FOR MAUI
        public RowDefinitionCollection TableRows
        {
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star), size).ToArray());
        }
        public ColumnDefinitionCollection TableCols
        {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), size).ToArray());
        }
        #endregion

        #region Events
        public event EventHandler? NewGame;
        public event EventHandler? GameCreating;
        public event EventHandler? LoadGame;
        public event EventHandler? SaveGame;
        public event EventHandler? ExitGame;
        #endregion

        #region Constructors
        public BlackHoleGameViewModel(BlackHoleGameModel model)
        {
            gameModel = model;
            gameModel.FieldHasChanged += new EventHandler<BlackHoleFieldEventArgs>(Model_OnFieldChange);
            gameModel.Victory += new EventHandler<int>(Model_Victory);
            gameModel.TableLoaded += new EventHandler<BlackHoleLoadEventArgs>(Model_Load);
            gameModel.PartyChanged += new EventHandler(Model_PartyHasChanged);
            gameModel.ShipEntered += new EventHandler<int>(Model_ShipHasEntered);

            IWantNewGame = new DelegateCommand(param => OnWantNewGame());
            NewGameCommand = new DelegateCommand(param => OnNewGame(Convert.ToInt32(param)));
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitGameCommand = new DelegateCommand(param => OnExitGame());

            Fields = new ObservableCollection<BlackHoleField>();
        }
        #endregion

        #region Event Handlers
        private void Model_OnFieldChange(object? sender, BlackHoleFieldEventArgs e)
        {
            //MessageBox.Show($"{e.Y}, {e.X}: {CalculateCoord(e.Y,e.X)}","Event Target", MessageBoxButton.OK);
            //MessageBox.Show($"{DetermineColorString(e.Value)}", "Event color", MessageBoxButton.OK);
            if (e.Value == 4 || e.Value == 5)
            {
                if (e.Y > 0)//felette lévő sor
                {

                    Fields[CalculateCoord(e.Y - 1, e.X)].Colour = DetermineSelectedColor(Fields[CalculateCoord(e.Y - 1, e.X)].Colour, e.Value);
                }
                if (e.Y < gameModel.gameSize - 1)//alatta lévő sor
                {
                    Fields[CalculateCoord(e.Y + 1, e.X)].Colour = DetermineSelectedColor(Fields[CalculateCoord(e.Y + 1, e.X)].Colour, e.Value);
                }
                if (e.X > 0 && e.X % gameModel.gameSize != 0)//előtte lévő oszlop
                {
                    Fields[CalculateCoord(e.Y, e.X - 1)].Colour = DetermineSelectedColor(Fields[CalculateCoord(e.Y, e.X - 1)].Colour, e.Value);
                }
                if (e.X < gameModel.gameSize - 1 && e.X % gameModel.gameSize != gameModel.gameSize - 1)//utána lévő oszlop
                {
                    Fields[CalculateCoord(e.Y, e.X + 1)].Colour = DetermineSelectedColor(Fields[CalculateCoord(e.Y, e.X + 1)].Colour, e.Value);
                }
            }
            else
            {
                Fields[CalculateCoord(e.Y, e.X)].Colour = DetermineColor(e.Value);
            }
        }
        private void Model_Victory(object? sender, int winner)
        {
            IsSaveOkay = false;
            OnPropertyChanged(nameof(IsSaveOkay));
            foreach (BlackHoleField field in Fields)
            {
                field.IsEnabled = false;
            }
        }
        private void Model_Load(object? sender, BlackHoleLoadEventArgs e)
        {
            GenerateTable(e.size);
            OnPropertyChanged(nameof(TableRows));
            OnPropertyChanged(nameof(TableCols));
            OnPropertyChanged(nameof(IsActivePlayerBlue));
            OnPropertyChanged(nameof(IsActivePlayerRed));
        }
        private void Model_PartyHasChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsActivePlayerBlue));
            OnPropertyChanged(nameof(IsActivePlayerRed));
        }
        private void Model_ShipHasEntered(object? sender, int party)
        {
            switch (party)
            {
                case 1:
                    OnPropertyChanged(nameof(RedShipsNumber));
                    break;
                case 2:
                    OnPropertyChanged(nameof(BlueShipsNumber));
                    break;
            }
        }
        #endregion

        #region Event Methods
        private void OnWantNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnNewGame(int param)
        {
            GameCreating?.Invoke(this, EventArgs.Empty);
            GenerateTable(param);
            gameModel.NewGame(param);
            OnPropertyChanged(nameof(TableRows));
            OnPropertyChanged(nameof(TableCols));
            OnPropertyChanged(nameof(RedShipsNumber));
            OnPropertyChanged(nameof(BlueShipsNumber));
        }
        private void OnLoadGame()
        {
            LoadGame?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(RedShipsNumber));
            OnPropertyChanged(nameof(BlueShipsNumber));
            IsSaveOkay = true;
            OnPropertyChanged(nameof(IsSaveOkay));
        }
        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Methods
        public void GenerateTable(int size)
        {
            this.size = size;
            Fields.Clear();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Fields.Add(new BlackHoleField()
                    {
                        IsEnabled = true,
                        X = j,
                        Y = i,
                        Colour = Colors.White,
                        Click = new DelegateCommand(
                            param =>
                            {
                                if (param is Tuple<int, int> pos)
                                {
                                    gameModel.Click(pos.Item1, pos.Item2);
                                }
                            }
                            )
                    });
                }
            }

            IsSaveOkay = true;
            OnPropertyChanged(nameof(IsSaveOkay));

            OnPropertyChanged(nameof(TableRows));
            OnPropertyChanged(nameof(TableCols));
        }
        private Color DetermineColor(int type)
        {
            switch (type)
            {
                case 0://semmi
                    return Colors.White;
                case 1://piros
                    return Colors.Red;
                case 2://kék
                    return Colors.Blue;
                case 3://feketelyuk
                    return Colors.Black;
                default://ezzel biztos valami error van
                    return Colors.Pink;
            }
        }
        private Color DetermineSelectedColor(Color set, int type)
        {
            if (set == Colors.White)
            {
                if (type == 4)
                {
                    return Colors.Crimson;
                }
                else
                {
                    return Colors.Aqua;
                }
            }
            else
            {
                if (set != Colors.Red && set != Colors.Blue && set != Colors.Black)
                {
                    return Colors.White;
                }
                return set;
            }
        }
        private int CalculateCoord(int x, int y)
        {
            return (x * gameModel.gameSize) + y;
        }
        #endregion
    }
}
