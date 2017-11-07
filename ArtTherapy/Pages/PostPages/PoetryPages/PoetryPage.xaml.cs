using ArtTherapy.Models.ProductModels;
using ArtTherapy.ViewModels;
using ArtTherapyCore.Extensions;

using Microsoft.Toolkit.Uwp.UI.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ArtTherapy.Pages.PostPages.PoetryPages
{
    public interface ISetLoadingType
    {
        void SetJsonLoadingType(object parameter);
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

        public void Execute(object parameter) => _ViewModel.SetJsonLoadingType(parameter);
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

        public ProductViewModel<ProductModel> ViewModel
        {
            get => _ViewModel;
            set => SetValue(ref _ViewModel, value);
        }
        private ProductViewModel<ProductModel> _ViewModel;
        public PoetryPage()
        {
            this.InitializeComponent();
            Id = 2;
            Title = "Стихи";
            NavigateEventType = NavigateEventTypes.ListBoxSelectionChanged;
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
                    ViewModel = new ProductViewModel<ProductModel>(AppSettings.AppSettings.Current.LoadingType);
                    ViewModel.Loaded += _viewModel_Loaded;
                    switch (AppSettings.AppSettings.Current.LoadingType)
                    {
                        case LoadingType.FullMode: if (r1 != null) r1.IsChecked = true; break;
                        case LoadingType.NoImageMode: if (r2 != null) r2.IsChecked = true; break;
                        case LoadingType.PriceMode: if (r3 != null) r3.IsChecked = true; break;
                        case LoadingType.None: if (r4 != null) r4.IsChecked = true; break;
                    }
                    if (loading != null)
                        loading.IsActive = false;
                    Initialized?.Invoke(this, new EventArgs());
                });

                if (ViewModel != null)
                {
                    await ViewModel.LoadData(1);
                    SetJsonLoadingType(AppSettings.AppSettings.Current.LoadingType);
                }
            });
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            ViewModel.Loaded -= _viewModel_Loaded;
        }

        private async void _viewModel_Loaded(object sender, PostEventArgs e)
        {
            double value = 0;
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                value = scrollViewer.GetScrollViewProgress();
            });

            if (!e.IsFullInitialized)
                if (ViewModel != null)
                    await ViewModel.LoadData(value);
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            double value = scrollViewer.GetScrollViewProgress();
            if (ViewModel != null)
                await ViewModel.LoadData(value);
        }

        private async void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double value = scrollViewer.GetScrollViewProgress();
            if (ViewModel != null)
                await ViewModel.LoadData(value);
        }

        public async void SetJsonLoadingType(object parameter)
        {
            int result;
            var stringValue = parameter as String;
            if (!String.IsNullOrEmpty(stringValue))
                if (int.TryParse(stringValue, out result))
                {
                    await AppSettings.AppSettings.Current.Set((LoadingType)result);
                    await SetLoadingType((LoadingType)result);
                    double value = scrollViewer.GetScrollViewProgress();
                    if (ViewModel != null)
                        await ViewModel.LoadData(value);
                }
        }

        private async Task SetLoadingType(LoadingType loadingType)
        {
            var panel = gridView.ItemsPanelRoot;
            if (panel == null) return;

            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                for (int i = 0; i < panel.Children.Count; i++)
                {
                    var gridViewItem = panel.Children[i] as GridViewItem;
                    if (gridViewItem == null) return;
                    if (gridViewItem.ContentTemplateRoot == null) return;

                    var demoControl = gridViewItem.ContentTemplateRoot as DemoControl;
                    if (demoControl == null) return;

                    demoControl.UpdateState();
                    demoControl.UpdateLayout();
                }
            });
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

        private async void DemoControl_ProductTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Debug.WriteLine("DemoControl_ProductTapped");
            var currentProduct = sender as CurrentProductModel;
            if (currentProduct != null)
                await new MessageDialog(currentProduct.Sku).ShowAsync();
        }

        private async void DemoControl_ProductInfoTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            Debug.WriteLine("DemoControl_ProductInfoTapped");
            var currentProduct = sender as CurrentProductModel;
            if (currentProduct != null)
                await new MessageDialog(currentProduct.Sku).ShowAsync();
        }

        private async void DemoControl_BasketTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Debug.WriteLine("DemoControl_BasketTapped");
            var currentProduct = sender as CurrentProductModel;
            if (currentProduct != null)
                await new MessageDialog(currentProduct.Sku).ShowAsync();
        }

        private async void DemoControl_BasketInfoTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            Debug.WriteLine("DemoControl_BasketInfoTapped");
            var currentProduct = sender as CurrentProductModel;
            if (currentProduct != null)
                await new MessageDialog(currentProduct.Sku).ShowAsync();
        }
    }
}
