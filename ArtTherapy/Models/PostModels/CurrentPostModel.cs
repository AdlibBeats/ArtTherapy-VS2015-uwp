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
            get => _Id;
            set => SetValue(ref _Id, value);
        }
        private uint _Id;

        public string BuyIcon
        {
            get => _BuyIcon;
            set => SetValue(ref _BuyIcon, value);
        }
        private string _BuyIcon;

        public string Name
        {
            get => _Name;
            set => SetValue(ref _Name, value);
        }
        private string _Name;

        public string Description
        {
            get => _Description;
            set => SetValue(ref _Description, value);
        }
        private string _Description;

        public string Text
        {
            get => _Text;
            set => SetValue(ref _Text, value);
        }
        private string _Text;
        
        public string Type
        {
            get => _Type;
            set => SetValue(ref _Type, value);
        }
        private string _Type;

        #endregion

        #region JsonIgnore

        [JsonIgnore]
        public ImageSource Image
        {
            get => _Image;
            set => SetValue(ref _Image, value);
        }
        private ImageSource _Image;

        [JsonIgnore]
        public bool IsLoading
        {
            get => _IsLoading;
            set => SetValue(ref _IsLoading, value);
        }
        private bool _IsLoading;

        [JsonIgnore]
        public bool IsLoadingImage
        {
            get => _IsLoadingImage;
            set => SetValue(ref _IsLoadingImage, value);
        }
        private bool _IsLoadingImage;

        [JsonIgnore]
        public bool IsLoadingPrices
        {
            get => _IsLoadingPrices;
            set => SetValue(ref _IsLoadingPrices, value);
        }
        private bool _IsLoadingPrices;

        [JsonIgnore]
        public bool IsLoadingRemains
        {
            get => _IsLoadingRemains;
            set => SetValue(ref _IsLoadingRemains, value);
        }
        private bool _IsLoadingRemains;

        [JsonIgnore]
        public bool IsEnabledBuy
        {
            get => _IsEnabledBuy;
            set => SetValue(ref _IsEnabledBuy, value);
        }
        private bool _IsEnabledBuy;

        #endregion
    }
}
