using System;
using System.Collections.Generic;
using System;

namespace BlackHole.ViewModel
{
    public class StoredGameViewModel : ViewModelBase
    {
        #region Fields
        private string name = String.Empty;
        private DateTime modified;
        #endregion

        #region Property
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        public DateTime Modified
        {
            get { return modified; }
            set
            {
                if (modified != value)
                {
                    modified = value;
                    OnPropertyChanged(nameof(Modified));
                }
            }
        }
        public DelegateCommand? LoadGameCommand { get; set; }
        public DelegateCommand? SaveGameCommand { get; set; }
        #endregion
    }
}
