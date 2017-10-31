using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ArtTherapyCore.BaseModels
{
    public enum JsonLoadingType : byte
    {
        GetCount,
        GetFullData,
        GetImagesData,
        GetPricesData,
        GetRemainsData
    }

    public class JsonLoadingHelper
    {

        public JsonLoadingHelper(JsonLoadingType loadingType)
        {
            LoadingType = loadingType;
        }

        public string Path
        {
            get
            {
                switch (LoadingType)
                {
                    case JsonLoadingType.GetCount: return "PostModelCount.json";
                    case JsonLoadingType.GetFullData: return "PostModel.json";
                    case JsonLoadingType.GetImagesData: return "PostModelImages.json";
                    case JsonLoadingType.GetPricesData: return "PostModelPrices.json";
                    case JsonLoadingType.GetRemainsData: return "PostModelRemains.json";
                    default: return String.Empty;
                }
            }
        }

        public JsonLoadingType LoadingType { get; set; }
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
