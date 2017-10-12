using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ArtTherapyCore.BaseModels
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SetValue<T>(ref T oldValue, T newValue, [CallerMemberName]string propertyName = null)
        {
            oldValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
