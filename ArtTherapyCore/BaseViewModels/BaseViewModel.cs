using ArtTherapyCore.BaseModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace ArtTherapyCore.BaseViewModels
{
    public interface IBaseViewModel<out T> : INotifyPropertyChanged where T : BaseModel
    {
        T GetModel();
    }

    public class BaseViewModel<T> : DependencyObject, IBaseViewModel<T> where T : BaseModel
    {
        public BaseViewModel()
        {

        }
        
        public virtual T GetModel() { return default(T); }

        public Action Command
        {
            get => _Command;
            set => SetValue(ref _Command, value);
        }
        private Action _Command;

        public Action<object> CommandParameter
        {
            get => _CommandParameter;
            set => SetValue(ref _CommandParameter, value);
        }
        private Action<object> _CommandParameter;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SetValue<V>(ref V oldValue, V newValue, [CallerMemberName]string propertyName = null)
        {
            oldValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
