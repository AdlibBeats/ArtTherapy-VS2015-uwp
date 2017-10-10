using ArtTherapy.Models;
using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace ArtTherapy.ViewModels
{
    public interface IBaseViewModel<out T> : INotifyPropertyChanged where T : BaseModel, new()
    {
        T GetModel();
    }

    public abstract class BaseViewModel<T> : DependencyObject, IDisposable, IBaseViewModel<T> where T : BaseModel, new()
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
