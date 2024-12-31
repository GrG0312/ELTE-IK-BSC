using Avalonia.Media;
using Avalonia.Threading;
using BaseDefense.Model;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BaseDefense.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private GameModel model;

    public int GameSize { get => GameModel.GAME_SIZE; }
    public int BaseHP { get => model.BaseHp; }
    public string Points { get => model.PointsForSoldier.ToString(); }
    public ObservableCollection<Field> Fields { get; private set; }
    public RelayCommand NewGameCommand { get; private set; }
    public MainViewModel(GameModel m)
    {
        Fields = new ObservableCollection<Field>();

        this.model = m;
        model.FieldChanged += (object? sender, FieldValueEventArgs e) 
            => Dispatcher.UIThread.Invoke(() => Model_FieldChanged(sender, e));
        model.BaseHpChanged += Model_BaseHpChanged;
        model.PointsForSoldierChanged += Model_PointsChanged;
        NewGameCommand = new RelayCommand(() => 
        {
            NewGame();
        });
        NewGame();
    }

    #region Model Event Handlers
    private void Model_PointsChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(Points));
    }
    private void Model_BaseHpChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(BaseHP));
    }
    private void Model_FieldChanged(object? sender, FieldValueEventArgs e)
    {
        Field changed = Fields.Single(f => f.X == e.X && f.Y == e.Y);
        switch (e.Value)
        {
            case FieldType.EMPTY:
                changed.BackgroundColor = new SolidColorBrush(Avalonia.Media.Color.Parse("White"));
                break;
            case FieldType.BASE:
                changed.BackgroundColor = new SolidColorBrush(Avalonia.Media.Color.Parse("Green"));
                break;
            case FieldType.SOLDIER:
                changed.BackgroundColor = new SolidColorBrush(Avalonia.Media.Color.Parse("Blue"));
                break;
            case FieldType.ENEMY:
                changed.BackgroundColor = new SolidColorBrush(Avalonia.Media.Color.Parse("Red"));
                break;
            default:
                break;
        }
    }
    #endregion

    private void NewGame()
    {
        Fields.Clear();
        for (int i = 0; i < GameModel.GAME_SIZE; i++)
        {
            for (int j = 0; j < GameModel.GAME_SIZE; j++)
            {
                Field f = new Field(i, j, model.Click);
                Fields.Add(f);
            }
        }
        model.NewGame();
        OnPropertyChanged(nameof(BaseHP));
        OnPropertyChanged(nameof(Points));
    }
}
