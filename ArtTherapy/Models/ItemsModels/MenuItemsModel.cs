using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;

namespace ArtTherapy.Models.ItemsModels
{
    public class MenuItemsModel : BaseModel
    {
        public bool CanGoBack
        {
            get { return _CanGoBack; }
            set
            {
                _CanGoBack = value;
                OnPropertyChanged(nameof(CanGoBack));
            }
        }
        private bool _CanGoBack;

        public bool IsMenuPaneOpen
        {
            get { return _IsMenuPaneOpen; }
            set
            {
                _IsMenuPaneOpen = value;
                OnPropertyChanged(nameof(IsMenuPaneOpen));
            }
        }
        private bool _IsMenuPaneOpen;

        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }
        private int _SelectedIndex;

        public CollectionViewSource Items
        {
            get { return _Items; }
            set
            {
                _Items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        private CollectionViewSource _Items;
    }
}
