using System.Configuration;
using System.Data;
using System.Windows;
using Bomberman_Prototype1.ViewModel;
using Bomberman_Prototype1.Model;
using Bomberman_Prototype1.View;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.Common;
using System.Windows.Threading;
using Bomberman_Prototype1.Persistence;
using System.Linq.Expressions;
using System.Security.AccessControl;

namespace Bomberman_Prototype1;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    #region Fields
    private GameModel? model;
    private ViewModelMain? viewModel;
    private MainWindow? view;
    private DispatcherTimer? timer;

    private KeyBinding[]? appKeyBindings;
    private ProfileLoader inFile = new ProfileLoader(); //profilok beolvasásához
    #endregion
    public App()
    {
        Startup += new StartupEventHandler(App_Startup);
    }

    #region App Event Handlers
    private void App_Startup(object sender, StartupEventArgs eventArgs)
    {
        //modell létrehozása
        model = new GameModel();

        //viewmodel létrehozása
        viewModel = new ViewModelMain(model);
        viewModel.ChangeDisplayedWindow += ViewModel_ChangeDisplay;
        viewModel.SelectedOrCanceledProfile += new EventHandler<string>(ViewModel_SelectionEnded);
        viewModel.CreateOrCanceledProfile += new EventHandler<string>(ViewModel_CreationEnded);
        viewModel.LoadNext += new EventHandler<string>(ViewModel_LoadNext);


        viewModel.PlayerJoinedLobby += new EventHandler<string>(ViewModel_PlayerJoined);

        //Profile reading
        inFile.Load();
        viewModel.CurrentProfile = inFile.GetCurrentIndexedProfile();
        viewModel.CurrentPictureForNewProfile = inFile.GetCurrentIndexedPicture();

        //nézet létrehozása
        view = new();
        view.Closed += (object? sender, EventArgs e) =>
        {
            viewModel.Shutdown();
            inFile.Save(viewModel.CurrentProfile);
            App.Current.Shutdown();
        };
        view.DataContext = viewModel;
        view.Content = new MainMenuPage();
        view.Show();

        //timer beállítása
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(1000);
        timer.Tick += model.AdvanceTime;

        //keybinds
        appKeyBindings = new KeyBinding[]
        {
            new KeyBinding(){ Key = Key.W, Command = viewModel.CharacterMoveCommand, CommandParameter = 0 },
            new KeyBinding(){ Key = Key.Up, Command = viewModel.CharacterMoveCommand, CommandParameter = 0 },

            new KeyBinding(){ Key = Key.S, Command = viewModel.CharacterMoveCommand, CommandParameter = 1 },
            new KeyBinding(){ Key = Key.Down, Command = viewModel.CharacterMoveCommand, CommandParameter = 1 },

            new KeyBinding(){ Key = Key.A, Command = viewModel.CharacterMoveCommand, CommandParameter = 2 },
            new KeyBinding(){ Key = Key.Left, Command = viewModel.CharacterMoveCommand, CommandParameter = 2 },

            new KeyBinding(){ Key = Key.D, Command = viewModel.CharacterMoveCommand, CommandParameter = 3 },
            new KeyBinding(){ Key = Key.Right, Command = viewModel.CharacterMoveCommand, CommandParameter = 3 },

            new KeyBinding(){ Key = Key.Space, Command = viewModel.PlaceBombCommand },
            new KeyBinding(){ Key = Key.O, Command = viewModel.PutObstacleCommand },
        };
    }

    #endregion

    #region ViewModel Event Handlers
    /// <summary>
    /// This method will handle the changes in display and will modify the View and View InputBindings accordingly
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="state">The Window that should be displayed in the View</param>
    private void ViewModel_ChangeDisplay(object? sender, ViewModelMain.ViewState state)
    {
        switch (state)
        {
            case ViewModelMain.ViewState.RoundScreen:
                EndGamePopup roundpopup = new EndGamePopup();
                roundpopup.DataContext = viewModel;
                roundpopup.ShowDialog();
                viewModel!.StartNewRound();
                break;
            case ViewModelMain.ViewState.EndScreen:
                EndGamePopup endpopup = new EndGamePopup();
                endpopup.DataContext = viewModel;
                endpopup.ShowDialog();
                //Le kéne menteni a profilokat
                inFile.Save(viewModel!.CurrentProfile!);
                viewModel!.ChangeDisplayedWindow?.Invoke(this, ViewModelMain.ViewState.MainMenu);
                break;
            case ViewModelMain.ViewState.MainMenu:
                view!.InputBindings.Clear();
                view.Content = new MainMenuPage();
                break;
            case ViewModelMain.ViewState.JoinGame:
                JoinGamePage join = new JoinGamePage();
                if (join.ShowDialog() == true)
                {
                    try
                    {
                        viewModel!.JoinGame(join.Address);
                    }
                    catch(Exception)
                    {
                        viewModel!.ChangeDisplayedWindow?.Invoke(this, ViewModelMain.ViewState.JoinGame);
                    }
                }
                break;
            case ViewModelMain.ViewState.HostGame:
                view!.Content = new HostGamePage();
                break;
            case ViewModelMain.ViewState.SettingUpGame:
                NewGamePopup popup = new NewGamePopup();
                if (popup.ShowDialog() == true)
                {
                    viewModel!.HostGame(popup.Params.playerNumber,popup.Params.roundNumber, popup.Params.mapID, popup.Params.isBattleRoyale);
                }
                break;
            case ViewModelMain.ViewState.InGame:
                view!.Focus();
                view.Content = new GamePage();
                view.WindowState = WindowState.Maximized;
                foreach (KeyBinding bind in appKeyBindings!)
                {
                    view.InputBindings.Add(bind);
                }
                timer!.Start();
                break;
            case ViewModelMain.ViewState.BrowsingProfiles:
                view!.Content = new ProfilePage();
                inFile.Load();
                viewModel!.CurrentProfile = inFile.GetCurrentIndexedProfile();
                break;
            case ViewModelMain.ViewState.CreateProfile:
                view!.Content = new CreateProfile();
                break;
            default:
                break;
        }
    }
    private void ViewModel_PlayerJoined(object? sender, string msg)
    {
        HostGamePage hp = (HostGamePage)view!.Content;
        hp.Responder.AppendText('\n' + msg + '\n');
    }

    private void ViewModel_GameOver(object? sender, string s)
    {
        if (s == "lost") 
        { 
            MessageBox.Show("Sorry, you lost!", "The game is over!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        else
        {
            MessageBox.Show("Congratulations, you won!", "The game is over!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        viewModel!.ChangeDisplayedWindow?.Invoke(this, ViewModelMain.ViewState.MainMenu);
    }
    #region Profile Navigation
    private void ViewModel_SelectionEnded(object? sender, string e)
    {
        if (e == "selected")
        {
            inFile.Update();
            viewModel!.CurrentProfile = inFile.GetCurrentIndexedProfile();
        }
        viewModel!.ChangeDisplayedWindow?.Invoke(this, ViewModelMain.ViewState.MainMenu);
    }
    private void ViewModel_LoadNext(object? sender, string e)
    {
        if (e == "profile") { viewModel!.CurrentProfile = inFile.GetNextProfileByIndex(); }
        else { viewModel!.CurrentPictureForNewProfile=inFile.GetNextPictureByIndex(); }
    }
    private void ViewModel_CreationEnded(object? sender, string e)
    {
        if (e == "create")
        {
            CreateProfile? profilePage = view!.Content as CreateProfile;
            inFile.Save(new Profile(profilePage!.NewProfileName.Text, 0, 0, inFile.GetCurrentIndexedPicture()));
        }
        viewModel!.ChangeDisplayedWindow?.Invoke(this, ViewModelMain.ViewState.MainMenu);
    }
    #endregion

    #endregion
}