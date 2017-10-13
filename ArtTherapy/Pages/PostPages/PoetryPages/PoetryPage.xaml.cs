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
using ArtTherapy.Models.PostModels;
using ArtTherapyUI.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI;
using ArtTherapyCore.Extensions;
using System.Runtime.CompilerServices;
using ArtTherapyCore.BaseViewModels;
using ArtTherapyCore.BaseModels;

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

        public PostViewModel<PostModel> ViewModel
        {
            get => _ViewModel;
        }
        private PostViewModel<PostModel> _ViewModel;

        public event EventHandler<EventArgs> Initialized;

        public PoetryPage()
        {
            this.InitializeComponent();

            Id = 2;
            Title = "Стихи";
            NavigateEventType = NavigateEventTypes.ListBoxSelectionChanged;
            _ViewModel =  new PostViewModel<PostModel>();

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

            _ViewModel.Loaded += _viewModel_Loaded;

            Initialized?.Invoke(this, new EventArgs());
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            _ViewModel.Loaded -= _viewModel_Loaded;

            _ViewModel.Dispose();
        }

        private void _viewModel_Loaded(object sender, PostEventArgs e)
        {
            if (!e.IsFullInitialized)
                _ViewModel.LoadData(scrollViewer.GetScrollViewProgress());
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            _ViewModel.LoadData(scrollViewer.GetScrollViewProgress());
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _ContentDialog.Width = Window.Current.Bounds.Width;
            _ContentDialog.Height = Window.Current.Bounds.Height;

            _ViewModel.LoadData(scrollViewer.GetScrollViewProgress());
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetValue<T>(ref T oldValue, T newValue, [CallerMemberName]string propertyName = null)
        {
            oldValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        ContentDialog _ContentDialog = new ContentDialog();
        DispatcherTimer _DispatcherTimer = new DispatcherTimer();

        int angle = 1;
        bool isLocy = false;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
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
            var textBlock = new TextBlock();

            var adaptiveGridView = sender as AdaptiveGridView;
            if (adaptiveGridView != null)
            {
                var selectedItem = adaptiveGridView.SelectedItem as CurrentPostModel;
                if (selectedItem != null && selectedItem.Id > 0)
                {
                    textBlock.FontSize = 24d;
                    textBlock.TextWrapping = TextWrapping.WrapWholeWords;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.Foreground = new SolidColorBrush(Colors.White);
                    textBlock.Text = $"{selectedItem.Id}: {selectedItem.Name}";

                    _ContentDialog.Content = textBlock;
                    await _ContentDialog.ShowAsync();
                }
            }
        }
    }
}
