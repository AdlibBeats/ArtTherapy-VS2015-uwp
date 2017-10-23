using ArtTherapy.ViewModels;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ArtTherapy.Models.ProductModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ArtTherapyCore.Extensions;
using System.Diagnostics;

namespace ArtTherapy.Pages.PostPages.PoetryPages
{
    public sealed partial class PoetryPage : Page, IPage
    {
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

        public PostViewModel<ProductModel> ViewModel
        {
            get => _ViewModel;
        }
        private PostViewModel<ProductModel> _ViewModel;

        public event EventHandler<EventArgs> Initialized;

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
            _ContentDialog.Width = Window.Current.Bounds.Width;
            _ContentDialog.Height = Window.Current.Bounds.Height;

            _ContentDialog.PointerPressed += (s, args) =>
            {
                _ContentDialog.Hide();
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ArtTherapy.AppSettings.AppSettings.Current.Get();

            _ViewModel = new PostViewModel<ProductModel>(AppSettings.AppSettings.Current.LoadingType);

            _ViewModel.Loaded += _viewModel_Loaded;

            switch (_ViewModel.LoadingType)
            {
                case LoadingType.FullMode: r1.IsChecked = true; break;
                case LoadingType.NoImageMode: r2.IsChecked = true; break;
                case LoadingType.OnlyPriceMode: r3.IsChecked = true; break;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            _ViewModel.Loaded -= _viewModel_Loaded;

            _ViewModel.Dispose();
        }

        private void _viewModel_Loaded(object sender, PostEventArgs e)
        {
            double value = scrollViewer.GetScrollViewProgress();
            if (!e.IsFullInitialized)
                _ViewModel.LoadData(value);
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            double value = scrollViewer.GetScrollViewProgress();
            _ViewModel.LoadData(value);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _ContentDialog.Width = Window.Current.Bounds.Width;
            _ContentDialog.Height = Window.Current.Bounds.Height;

            double value = scrollViewer.GetScrollViewProgress();
            _ViewModel.LoadData(value);
        }

        ContentDialog _ContentDialog = new ContentDialog();
        DispatcherTimer _DispatcherTimer = new DispatcherTimer();

        int angle = 1;
        bool isLocy = false;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Initialized?.Invoke(this, new EventArgs());

            //_ViewModel = new PostViewModel<ProductModel>(loadImages.IsOn);

            //_DispatcherTimer.Interval = TimeSpan.FromSeconds(0.005);
            //_DispatcherTimer.Tick += (s, args) =>
            //{
            //    Task.Factory.StartNew(async () =>
            //    {
            //        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //        {
            //            foreach (Control frame in gridView.ItemsPanelRoot.Children)
            //            {
            //                if (frame != null)
            //                {
            //                    if (!(frame.RenderTransform is RotateTransform))
            //                        frame.RenderTransform = new RotateTransform();

            //                    (frame.RenderTransform as RotateTransform).CenterX = 0.5 * frame.ActualWidth;
            //                    (frame.RenderTransform as RotateTransform).CenterY = 0.5 * frame.ActualHeight;

            //                    (frame.RenderTransform as RotateTransform).Angle += angle;
            //                }
            //            }
            //            if (!isLocy)
            //                ++angle;
            //            else
            //                --angle;

            //            if (angle == 50)
            //                isLocy = true;
            //            else if (angle == -50)
            //                isLocy = false;
            //        });
            //    });
            //};

            //_DispatcherTimer.Start();
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

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetValue<T>(ref T oldValue, T newValue, [CallerMemberName]string propertyName = null)
        {
            oldValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                var name = radioButton.Content as String;
                if (name != null)
                {
                    _ViewModel.Loaded -= _viewModel_Loaded;
                    _ViewModel.Dispose();
                    
                    switch (name)
                    {
                        case "С изображениями":
                            AppSettings.AppSettings.Current.Set(LoadingType.FullMode);
                            _ViewModel = new PostViewModel<ProductModel>(LoadingType.FullMode);
                            gridView.ItemHeight = 288;
                            break;
                        case "Без изображений":
                            AppSettings.AppSettings.Current.Set(LoadingType.NoImageMode);
                            _ViewModel = new PostViewModel<ProductModel>(LoadingType.NoImageMode);
                            gridView.ItemHeight = 175;
                            break;
                        case "Без Sku":
                            AppSettings.AppSettings.Current.Set(LoadingType.OnlyPriceMode);
                            _ViewModel = new PostViewModel<ProductModel>(LoadingType.OnlyPriceMode);
                            gridView.ItemHeight = 100;
                            break;
                    }

                    _ViewModel.Loaded += _viewModel_Loaded;
                    gridView.ItemsSource = _ViewModel.ProductModel.Items;
                    _ViewModel.LoadData(1);
                }
            }
        }
    }
}
