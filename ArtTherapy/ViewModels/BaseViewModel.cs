using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace ArtTherapy.ViewModels
{
    public abstract class BaseViewModel : DependencyObject, INotifyPropertyChanged
    {
        public BaseViewModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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
