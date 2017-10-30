using ArtTherapy.ViewModels;
using ArtTherapyCore.BaseModels;
using Newtonsoft.Json;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace ArtTherapy.Models.ProductModels
{
    public enum LoadingType 
    {
        FullMode,
        NoImageMode,
        OnlyPriceMode,
        None
    }

    public class CurrentProductModel : BaseModel
    {
        #region Json Serialized

        public string Sku
        {
            get => _Sku;
            set => SetValue(ref _Sku, value);
        }
        private string _Sku;

        public string Name
        {
            get => _Name;
            set => SetValue(ref _Name, value);
        }
        private string _Name;

        public int? Price
        {
            get => _Price;
            set => SetValue(ref _Price, value);
        }
        private int? _Price;

        public int? DiscountPrice
        {
            get => _DiscountPrice;
            set => SetValue(ref _DiscountPrice, value);
        }
        private int? _DiscountPrice;

        public string ImageUrl
        {
            get => _ImageUrl;
            set => SetValue(ref _ImageUrl, value);
        }
        private string _ImageUrl;
        
        public int Remains
        {
            get => _Remains;
            set => SetValue(ref _Remains, value);
        }
        private int _Remains;

        #endregion

        #region JsonIgnore

        [JsonIgnore]
        public LoadingType LoadingType
        {
            get => _LoadingType;
            set => SetValue(ref _LoadingType, value);
        }
        private LoadingType _LoadingType;

        [JsonIgnore]
        public int? PriceDifference
        {
            get => _PriceDifference;
            set => SetValue(ref _PriceDifference, value);
        }
        private int? _PriceDifference;

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
        public bool IsLoadingPrice
        {
            get => _IsLoadingPrice;
            set => SetValue(ref _IsLoadingPrice, value);
        }
        private bool _IsLoadingPrice;

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
