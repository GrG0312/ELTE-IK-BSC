using BlackHole.Model;
using BlackHole.Persistance;
using BlackHole.View;
using BlackHole.ViewModel;

namespace BlackHole
{
    public partial class AppShell : Shell
    {
        #region Fields
        private IBlackHoleDataManager dataManager;
        private readonly BlackHoleGameModel gameModel;
        private readonly BlackHoleGameViewModel viewModel;
        private readonly IStore store;
        private readonly StoredGameBrowserModel storedModel;
        private readonly StoredGameBrowserViewModel storedViewModel;
        #endregion
        public AppShell(IStore BHStore, IBlackHoleDataManager BHDataManager, BlackHoleGameModel BHgameModel, BlackHoleGameViewModel BHviewModel)
        {
            InitializeComponent();
            store = BHStore;
            dataManager = BHDataManager;
            gameModel = BHgameModel;
            viewModel = BHviewModel;

            gameModel.Victory += Model_Victory;

            viewModel.NewGame += ViewModel_WantNewGame;
            viewModel.GameCreating += ViewModel_GameCreating;
            viewModel.SaveGame += ViewModel_SaveGame;
            viewModel.LoadGame += ViewModel_LoadGame;

            storedModel = new StoredGameBrowserModel(store);
            storedViewModel = new StoredGameBrowserViewModel(storedModel);
            storedViewModel.GameLoading += StoredViewModel_GameLoading;
            storedViewModel.GameSaving += StoredViewModel_GameSaving;
        }

        #region Model Event handlers
        private async void Model_Victory(object? sender, int winner)
        {
            string name = winner == 1 ? "RED" : "BLUE";
            await DisplayAlert("BlackHole Game",
                "Congratulations!" +
                Environment.NewLine +
                $" Player \"{name}\" has won the game!",
                "OK");
        }
        #endregion

        #region ViewModel Event handlers
        private async void ViewModel_LoadGame(object? sender, EventArgs e)
        {
            await storedModel.UpdateAsync();
            await Navigation.PushAsync(new LoadPage
            {
                BindingContext = storedViewModel
            });
        }
        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            await storedModel.UpdateAsync();
            await Navigation.PushAsync(new SavePage
            {
                BindingContext = storedViewModel
            });
        }
        private async void ViewModel_WantNewGame(object? sender, EventArgs e)
        {
            await storedModel.UpdateAsync();
            await Navigation.PushAsync(new NewGamePage
            {
                BindingContext = viewModel
            });
        }
        private async void ViewModel_GameCreating(object? sender, EventArgs e)
        {
            await Navigation.PopAsync();
            await DisplayAlert("Black Hole Game", "Game created successfully, good luck!", "OK");
        }
        #endregion

        #region Stored ViewModel Event handlers
        private async void StoredViewModel_GameLoading(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync();

            try
            {
                await gameModel.LoadGameAsync(e.Name);
                await DisplayAlert("Black Hole Game", "Successful loading!", "OK");
            }
            catch
            {
                await DisplayAlert("Black Hole Game", "Oops!" + Environment.NewLine + "Something went wrong, could not load the saved game.", "OK");
            }
        }
        private async void StoredViewModel_GameSaving(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync();
            try
            {
                await gameModel.SaveGameAsync(e.Name);
                await DisplayAlert("BlackHole Game", "Game saved succesfully!", "OK");
            }
            catch
            {
                await DisplayAlert("BlackHole Game", "Oops!" + Environment.NewLine + "Something went wrong, could not save the game.", "OK");
            }
        }
        #endregion
    }
}
