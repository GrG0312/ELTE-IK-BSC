using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackHole.Persistance;

namespace BlackHole.Model
{
    public class StoredGameBrowserModel
    {
        private IStore store;

        public List<StoredGameModel> StoredGames { get; private set; }

        public event EventHandler? StoreChanged;

        public StoredGameBrowserModel(IStore store)
        {
            this.store = store;
            StoredGames = new List<StoredGameModel>();
        }

        public async Task UpdateAsync()
        {
            if (store == null)
            {
                return;
            }

            StoredGames.Clear();

            foreach (string name in await store.GetFilesAsync())
            {
                if (name == "SuspendedGame")
                {
                    continue;
                }

                StoredGames.Add(new StoredGameModel
                {
                    Name = name,
                    Modified = await store.GetModifiedTimeAsync(name)
                });
            }

            StoredGames = StoredGames.OrderByDescending(item => item.Modified).ToList();
            OnSaveChanged();
        }

        private void OnSaveChanged()
        {
            StoreChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
