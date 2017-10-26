using ArtTherapy.Models.ProductModels;
using ArtTherapy.ViewModels;
using ArtTherapyCore.Extensions;

using Microsoft.Toolkit.Uwp.UI.Controls;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ArtTherapy.Pages.PostPages.PoetryPages
{
    public interface ISetLoadingType
    {
        void SetLoadingType(object parameter);
    }

    public class SetLoadingTypeCommand : ICommand
    {
        private ISetLoadingType _ViewModel;

        public SetLoadingTypeCommand(ISetLoadingType viewModel)
        {
            _ViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => _ViewModel.SetLoadingType(parameter);
    }

    public sealed partial class PoetryPage : Page, IPage, ISetLoadingType
    {
        #region Public
        public uint Id
        {
            get => _Id;
            set => SetValue(ref _Id, value);
        }
        private uint _Id;

        public string Title
        {
            get => _Title;
            set => SetValue(ref _Title, value);
        }
        private string _Title;

        public NavigateEventTypes NavigateEventType
        {
            get => _NavigateEventType;
            set => SetValue(ref _NavigateEventType, value);
        }
        private NavigateEventTypes _NavigateEventType;

        public event EventHandler<EventArgs> Initialized;

        #endregion

        public PostViewModel<ProductModel> ViewModel
        {
            get => _ViewModel;
            set => SetValue(ref _ViewModel, value);
        }
        private PostViewModel<ProductModel> _ViewModel;

        private Task _LoadDataTask = null;
        private ContentDialog _ContentDialog = new ContentDialog();
        public PoetryPage()
        {
            this.InitializeComponent();
            Id = 2;
            Title = "Стихи";
            NavigateEventType = NavigateEventTypes.ListBoxSelectionChanged;

            _ContentDialog.Background = new SolidColorBrush(Colors.Black);
            _ContentDialog.BorderThickness = new Thickness(0);
            _ContentDialog.BorderBrush = _ContentDialog.Background;
            _ContentDialog.FullSizeDesired = true;
            _ContentDialog.MinWidth = 10;
            _ContentDialog.MinHeight = 10;
            _ContentDialog.MaxWidth = 5000;
            _ContentDialog.MaxHeight = 5000;

            _ContentDialog.PointerPressed += (s, args) =>
            {
                _ContentDialog.Hide();
            };

            SetLoadingTypeCommand = new SetLoadingTypeCommand(this);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            await Task.Run(async () =>
            {
                await AppSettings.AppSettings.Current.Get();
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    Debug.WriteLine(AppSettings.AppSettings.Current.LoadingType);
                    ViewModel = new PostViewModel<ProductModel>(AppSettings.AppSettings.Current.LoadingType);
                    ViewModel.Loaded += _viewModel_Loaded;
                    switch (AppSettings.AppSettings.Current.LoadingType)
                    {
                        case LoadingType.FullMode: if (r1 != null) r1.IsChecked = true; break;
                        case LoadingType.NoImageMode: if (r2 != null) r2.IsChecked = true; break;
                        case LoadingType.OnlyPriceMode: if (r3 != null) r3.IsChecked = true; break;
                        case LoadingType.None: if (r1 != null) r4.IsChecked = true; break;
                    }

                    double value = scrollViewer.GetScrollViewProgress();
                    _LoadDataTask = Task.Run(() => ViewModel.LoadData(value));
                    Initialized?.Invoke(this, new EventArgs());
                });
            });
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            ViewModel.Loaded -= _viewModel_Loaded;

            ViewModel.Dispose();
        }

        private async void _viewModel_Loaded(object sender, PostEventArgs e)
        {
            double value = 0;
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                value = scrollViewer.GetScrollViewProgress();
            });

            if (!e.IsFullInitialized)
                _LoadDataTask = Task.Run(() => ViewModel.LoadData(value));
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            double value = scrollViewer.GetScrollViewProgress();
            _LoadDataTask = Task.Run(() => ViewModel?.LoadData(value));
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _ContentDialog.Width = Window.Current.Bounds.Width;
            _ContentDialog.Height = Window.Current.Bounds.Height;

            double value = scrollViewer.GetScrollViewProgress();
            _LoadDataTask = Task.Run(() => ViewModel?.LoadData(value));
        }

        private async void gridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gridView.SelectedIndex > -1)
            {
                var textBlock = new TextBlock();

                var adaptiveGridView = sender as AdaptiveGridView;
                if (adaptiveGridView != null)
                {
                    var selectedItem = adaptiveGridView.SelectedItem as CurrentProductModel;
                    if (selectedItem != null && selectedItem.Sku > 0)
                    {
                        textBlock.FontSize = 24d;
                        textBlock.TextWrapping = TextWrapping.WrapWholeWords;
                        textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                        textBlock.VerticalAlignment = VerticalAlignment.Center;
                        textBlock.Foreground = new SolidColorBrush(Colors.White);
                        textBlock.Text = $"{selectedItem.Sku}: {selectedItem.Name}";

                        _ContentDialog.Content = textBlock;
                        await _ContentDialog.ShowAsync();
                    }
                }
                gridView.SelectedIndex = -1;
            }
        }

        public void SetLoadingType(object parameter)
        {
            int result;
            var stringValue = parameter as String;
            if (!String.IsNullOrEmpty(stringValue))
                if (int.TryParse(stringValue, out result))
                {
                    Task.Run(async () =>
                    {
                        await _LoadDataTask;
                        await AppSettings.AppSettings.Current.Set((LoadingType)result);
                        _LoadDataTask = Task.Run(() => ViewModel?.SetLoadingType((LoadingType)result));
                    });
                }
        }

        public ICommand SetLoadingTypeCommand
        {
            get => _SetLoadingTypeCommand;
            set => SetValue(ref _SetLoadingTypeCommand, value);
        }
        private ICommand _SetLoadingTypeCommand;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetValue<T>(ref T oldValue, T newValue, [CallerMemberName]string propertyName = null)
        {
            oldValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
