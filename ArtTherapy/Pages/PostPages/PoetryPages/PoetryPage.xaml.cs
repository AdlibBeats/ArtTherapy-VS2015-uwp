using ArtTherapy.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ArtTherapy.Pages.PostPages.PoetryPages
{
    public sealed partial class PoetryPage : Page, IPage
    {
        public uint Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        private uint _Id;

        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        private string _Title;

        public NavigateEventTypes NavigateEventType
        {
            get { return _NavigateEventType; }
            set
            {
                _NavigateEventType = value;
                OnPropertyChanged(nameof(NavigateEventType));
            }
        }
        private NavigateEventTypes _NavigateEventType;

        public event EventHandler<EventArgs> Initialized;

        public PoetryPage()
        {
            this.InitializeComponent();

            Id = 2;
            Title = "Стихи";
            NavigateEventType = NavigateEventTypes.ListBoxSelectionChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            DataContext = new PostViewModel("PoetryRepository");
            (DataContext as PostViewModel).Initialized += _viewModel_Initialized;
            Initialized?.Invoke(this, new EventArgs());
            scroll.ViewChanged += Scroll_ViewChanged;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        TaskCompletionSource<bool> isLoaded = new TaskCompletionSource<bool>();

        private void Scroll_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var newScrollProgress = (scroll.VerticalOffset + scroll.ActualHeight) / scroll.ExtentHeight;
            Debug.WriteLine(newScrollProgress);
            if (newScrollProgress >= 0.9)
            {
                Load();
            }
        }

        private async void Load()
        {
            await Task.Factory.StartNew(async () =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    (DataContext as PostViewModel).Initialize();
                    await isLoaded.Task;
                });
            });
        }

        private void _viewModel_Initialized(object sender, EventArgs e)
        {
            isLoaded.TrySetResult(true);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
