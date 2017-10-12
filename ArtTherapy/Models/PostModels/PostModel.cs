using ArtTherapyCore.BaseModels;
using System.Collections.ObjectModel;

namespace ArtTherapy.Models.PostModels
{
    public class PostModel : BaseModel
    {
        public ObservableCollection<CurrentPostModel> Items
        {
            get => _Items;
            set => _Items = value;
        }
        private ObservableCollection<CurrentPostModel> _Items;

        public int Count
        {
            get => _Count;
            set => SetValue(ref _Count, value);
        }
        private int _Count;
    }
}
