using BlackHole.Model;
using System;
using System.Collections.ObjectModel;

namespace BlackHole.ViewModel
{
    public class StoredGameBrowserViewModel : ViewModelBase
    {
        private StoredGameBrowserModel storedBrowserGameModel;

        public event EventHandler<StoredGameEventArgs>? GameLoading;
        public event EventHandler<StoredGameEventArgs>? GameSaving;

        public DelegateCommand NewSaveCommand { get; private set; }
        public ObservableCollection<StoredGameViewModel> StoredGames { get; private set; }
        public StoredGameBrowserViewModel(StoredGameBrowserModel model)
        {
            ArgumentNullException.ThrowIfNull(nameof(model));//??? igen?
            /*
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            */

            this.storedBrowserGameModel = model;
            this.storedBrowserGameModel.StoreChanged += new EventHandler(Model_StoreChanged);

            NewSaveCommand = new DelegateCommand(param =>
            {
                string? fileName = Path.GetFileNameWithoutExtension(param?.ToString()?.Trim());
                if (!String.IsNullOrEmpty(fileName))
                {
                    fileName += ".bht";
                    OnGameSaving(fileName);
                }
            });
            StoredGames = new ObservableCollection<StoredGameViewModel>();
            UpdateStoredGames();
        }

        private void UpdateStoredGames()
        {
            StoredGames.Clear();
            foreach (StoredGameModel item in storedBrowserGameModel.StoredGames)
            {
                StoredGames.Add(new StoredGameViewModel
                {
                    Name = item.Name,
                    Modified = item.Modified,
                    LoadGameCommand = new DelegateCommand(param => OnGameLoading(param?.ToString() ?? "")),
                    SaveGameCommand = new DelegateCommand(param => OnGameSaving(param?.ToString() ?? ""))
                });
            }
        }
        private void Model_StoreChanged(object? sender, EventArgs e)
        {
            UpdateStoredGames();
        }
        private void OnGameLoading(string name)
        {
            GameLoading?.Invoke(this, new StoredGameEventArgs { Name = name });
        }
        private void OnGameSaving(string name)
        {
            GameSaving?.Invoke(this, new StoredGameEventArgs { Name = name });
        }
    }
}
