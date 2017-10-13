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

    public class BaseViewModel<T> : DependencyObject, IDisposable, IBaseViewModel<T> where T : BaseModel
    {
        public BaseViewModel()
        {

        }
        
        public virtual T GetModel() { return default(T); }

        public virtual void Dispose() { }

        public Action Command
        {
            get => _command;
            set => SetValue(ref _command, value);
        }
        private Action _command;

        public Action<object> CommandParameter
        {
            get => _commandParameter;
            set => SetValue(ref _commandParameter, value);
        }
        private Action<object> _commandParameter;

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
