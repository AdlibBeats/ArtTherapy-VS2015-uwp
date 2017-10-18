using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ArtTherapyCore.BaseModels
{
    public class PriceHelper
    {
        public bool IsDiscountPrice { get; }
        public PriceHelper(bool isDiscountPrice)
        {
            IsDiscountPrice = isDiscountPrice;
        }
    }

    public enum LoadingType : byte
    {
        GetCount,
        GetFullData,
        GetImagesData,
        GetPricesData,
        GetRemainsData
    }

    public class LoadingHelper
    {

        public LoadingHelper(LoadingType loadingType)
        {
            LoadingType = loadingType;
        }

        public string Path
        {
            get
            {
                switch (LoadingType)
                {
                    case LoadingType.GetCount: return "PostModelCount.json";
                    case LoadingType.GetFullData: return "PostModel.json";
                    case LoadingType.GetImagesData: return "PostModelImages.json";
                    case LoadingType.GetPricesData: return "PostModelPrices.json";
                    case LoadingType.GetRemainsData: return "PostModelRemains.json";
                    default: return String.Empty;
                }
            }
        }

        public LoadingType LoadingType { get; set; }
    }
    public class CustomPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public LoadingHelper LoadingHelper { get; }
        public CustomPropertyChangedEventArgs(string propertyName, LoadingHelper loadingHelper) : base(propertyName)
        {
            LoadingHelper = loadingHelper;
        }
    }
    public interface IBaseModel : INotifyPropertyChanged
    {
        void SetValue<V>(ref V oldValue, V newValue, [CallerMemberName]string propertyName = null);
    }
    public class BaseModel : IBaseModel
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void SetValue<V>(ref V oldValue, V newValue, [CallerMemberName]string propertyName = null)
        {
            oldValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
