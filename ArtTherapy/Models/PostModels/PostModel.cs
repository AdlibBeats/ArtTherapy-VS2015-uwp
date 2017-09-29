using System.Collections.ObjectModel;

namespace ArtTherapy.Models.PostModels
{
    public class PostModel : BaseModel
    {
        public ObservableCollection<CurrentPostModel> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }
        private ObservableCollection<CurrentPostModel> _Items;

        public int Count
        {
            get { return _Count; }
            set
            {
                _Count = value;
                OnPropertyChanged(nameof(Count));
            }
        }
        private int _Count;
    }
}
