using ArtTherapyCore.BaseModels;
using Newtonsoft.Json;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace ArtTherapy.Models.PostModels
{
    public class CurrentPostModel : BaseModel
    {
        #region Json Serialized

        public uint Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        private uint _Id;


        public string BuyIcon
        {
            get { return _BuyIcon; }
            set
            {
                _BuyIcon = value;
                OnPropertyChanged(nameof(BuyIcon));
            }
        }
        private string _BuyIcon;

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _Name;

        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        private string _Description;

        public string Text
        {
            get { return _Text; }
            set
            {
                _Text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        private string _Text;
        
        public string Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        private string _Type;

        #endregion

        #region JsonIgnore

        [JsonIgnore]
        public ImageSource Image
        {
            get { return _Image; }
            set
            {
                _Image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
        private ImageSource _Image;

        [JsonIgnore]
        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        private bool _IsLoading;

        #endregion
    }
}
