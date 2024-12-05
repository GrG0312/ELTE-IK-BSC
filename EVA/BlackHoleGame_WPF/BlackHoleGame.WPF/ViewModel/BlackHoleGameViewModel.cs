using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using BlackHole.Model;

namespace BlackHole.ViewModel
{
    public class BlackHoleGameViewModel : ViewModelBase
    {
        private BlackHoleGameModel gameModel;

        #region Properties
        public ObservableCollection<BlackHoleField> Fields { get; set; }
        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public DelegateCommand ExitGameCommand { get; private set; }
        public Brush IsActivePlayerRed { get { return gameModel.currentParty == 1 ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.White); } }
        public Brush IsActivePlayerBlue { get { return gameModel.currentParty == 2 ? new SolidColorBrush(Colors.Blue) : new SolidColorBrush(Colors.White); } }
        public string RedShipsNumber { get {  return gameModel.GetEnteredShipsNumber(1).ToString(); } }
        public string BlueShipsNumber {  get { return gameModel.GetEnteredShipsNumber(2).ToString(); } }
        public bool IsSaveOkay { get; private set; }
        #endregion

        #region Events
        //public event EventHandler<int>? NewGame;
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

            NewGameCommand = new DelegateCommand(param => OnNewGame(Convert.ToInt32(param)));
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitGameCommand = new DelegateCommand(param => OnExitGame());

            Fields = new ObservableCollection<BlackHoleField>();

            OnNewGame(5);
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

                    Fields[CalculateCoord(e.Y - 1, e.X)].Colour = DetermineSelectedColor((SolidColorBrush)Fields[CalculateCoord(e.Y - 1, e.X)].Colour, e.Value);
                }
                if (e.Y < gameModel.gameSize - 1)//alatta lévő sor
                {
                    Fields[CalculateCoord(e.Y + 1, e.X)].Colour = DetermineSelectedColor((SolidColorBrush)Fields[CalculateCoord(e.Y + 1, e.X)].Colour, e.Value);
                }
                if (e.X > 0 && e.X % gameModel.gameSize != 0)//előtte lévő oszlop
                {
                    Fields[CalculateCoord(e.Y, e.X - 1)].Colour = DetermineSelectedColor((SolidColorBrush)Fields[CalculateCoord(e.Y, e.X - 1)].Colour, e.Value);
                }
                if (e.X < gameModel.gameSize - 1 && e.X % gameModel.gameSize != gameModel.gameSize - 1)//utána lévő oszlop
                {
                    Fields[CalculateCoord(e.Y, e.X + 1)].Colour = DetermineSelectedColor((SolidColorBrush)Fields[CalculateCoord(e.Y, e.X + 1)].Colour, e.Value);
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
        private void OnNewGame(int param)
        {
            //NewGame?.Invoke(this, param);
            GenerateTable(param);
            gameModel.NewGame(param);
            OnPropertyChanged(nameof(RedShipsNumber));
            OnPropertyChanged(nameof(BlueShipsNumber));
            IsSaveOkay = true;
            OnPropertyChanged(nameof(IsSaveOkay));
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
                        Colour = new SolidColorBrush(Colors.White),
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
        }
        private Brush DetermineColor(int type)
        {
            switch (type)
            {
                case 0://semmi
                    return new SolidColorBrush(Colors.White);
                case 1://piros
                    return new SolidColorBrush(Colors.Red);
                case 2://kék
                    return new SolidColorBrush(Colors.Blue);
                case 3://feketelyuk
                    return new SolidColorBrush(Colors.Black);
                default://ezzel biztos valami error van
                    return new SolidColorBrush(Colors.Pink);
            }
        }
        private Brush DetermineSelectedColor(SolidColorBrush set, int type)
        {
            SolidColorBrush white = new SolidColorBrush(Colors.White);
            SolidColorBrush red = new SolidColorBrush(Colors.Red);
            SolidColorBrush blue = new SolidColorBrush(Colors.Blue);
            SolidColorBrush black = new SolidColorBrush(Colors.Black);
            if (set.Color == white.Color)
            {
                if (type == 4)
                {
                    return new SolidColorBrush(Colors.Crimson);
                }
                else
                {
                    return new SolidColorBrush(Colors.Aqua);
                }
            }
            else
            {
                if (set.Color != red.Color && set.Color != blue.Color && set.Color != black.Color)
                {
                    return new SolidColorBrush(Colors.White);
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
