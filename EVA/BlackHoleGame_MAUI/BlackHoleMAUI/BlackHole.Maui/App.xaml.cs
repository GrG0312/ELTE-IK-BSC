using BlackHole.Model;
using BlackHole.Persistance;
using BlackHole.ViewModel;
using Microsoft.Maui.Controls;

namespace BlackHole
{
    public partial class App : Application
    {
        #region Fields
        //private const string SuspendedGameSavePath = "SuspendedGame.bht";

        private readonly AppShell shell;
        private readonly IBlackHoleDataManager dataManager;
        private readonly BlackHoleGameModel gameModel;
        private readonly IStore store;
        private readonly BlackHoleGameViewModel viewModel;
        #endregion
        public App()
        {
            InitializeComponent();
            store = new BlackHoleGameStore();
            dataManager = new BlackHoleDataHandler(FileSystem.AppDataDirectory);
            gameModel = new BlackHoleGameModel(dataManager);
            viewModel = new BlackHoleGameViewModel(gameModel);

            shell = new AppShell(store, dataManager,
                gameModel, viewModel)
            {
                BindingContext = viewModel
            };
            MainPage = shell;
        }

        #region Methods
        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = base.CreateWindow(activationState);

            window.Created += (s, e) =>
            {
                viewModel.GenerateTable(5);
                gameModel.NewGame(5);
            };
            return window;
        }
        #endregion
    }
}
