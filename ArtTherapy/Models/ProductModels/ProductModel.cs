using ArtTherapyCore.BaseModels;
using System.Collections.ObjectModel;

namespace ArtTherapy.Models.ProductModels
{
    public class ProductModel : BaseModel
    {
        public ObservableCollection<CurrentProductModel> Items
        {
            get => _Items;
            set => _Items = value;
        }
        private ObservableCollection<CurrentProductModel> _Items;

        public int Count
        {
            get => _Count;
            set => SetValue(ref _Count, value);
        }
        private int _Count;
    }
}
