using ArtTherapyCore.BaseModels;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;

namespace ArtTherapy.Models.ItemsModels
{
    public class MenuItemsModel : BaseModel
    {
        public bool CanGoBack
        {
            get => _CanGoBack;
            set => SetValue(ref _CanGoBack, value);
        }
        private bool _CanGoBack;

        public bool IsMenuPaneOpen
        {
            get => _IsMenuPaneOpen;
            set => SetValue(ref _IsMenuPaneOpen, value);
        }
        private bool _IsMenuPaneOpen;

        public int SelectedIndex
        {
            get => _SelectedIndex;
            set => SetValue(ref _SelectedIndex, value);
        }
        private int _SelectedIndex;

        public CollectionViewSource Items
        {
            get => _Items;
            set => SetValue(ref _Items, value);
        }
        private CollectionViewSource _Items;
    }
}
