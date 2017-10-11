using ArtTherapyCore.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ArtTherapyCore.BaseViewModels
{
    public interface IBaseViewModel<out T> : INotifyPropertyChanged where T : BaseModel
    {
        T GetModel();
    }

    public abstract class BaseViewModel<T> : DependencyObject, IDisposable, IBaseViewModel<T> where T : BaseModel
    {
        public BaseViewModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public abstract T GetModel();

        public abstract void Dispose();

        public Action Command
        {
            get => _command;
            set
            {
                _command = value;
                OnPropertyChanged(nameof(Command));
            }
        }
        private Action _command;

        public Action<object> CommandParameter
        {
            get => _commandParameter;
            set
            {
                _commandParameter = value;
                OnPropertyChanged(nameof(CommandParameter));
            }
        }
        private Action<object> _commandParameter;
    }
}
