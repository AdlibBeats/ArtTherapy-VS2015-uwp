using ArtTherapy.Models.ProductModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace ArtTherapy
{
    public class DemoControl : Control
    {
        //HISTORY: YouTubeAnimation Borders
        private Border _imageBorder;
        private Border _nameBorder;
        private Border _priceBorder;
        private Border _discountPriceBorder;
        private Border _remainsBorder;
        private Border _skuBorder;

        //HISTORY: Click Events
        private Grid _root;
        private Button _productTrueBuy;
        private Grid _rootGrid;
        
        public event TappedEventHandler ProductTapped;
        public event RightTappedEventHandler ProductInfoTapped;
        public event TappedEventHandler BasketTapped;
        public event RightTappedEventHandler BasketInfoTapped;

        public LoadingType LoadingType
        {
            get => (LoadingType)GetValue(LoadingTypeProperty);
            set => SetValue(LoadingTypeProperty, value);
        }

        public static readonly DependencyProperty LoadingTypeProperty =
            DependencyProperty.Register("LoadingType", typeof(LoadingType), typeof(DemoControl), new PropertyMetadata(LoadingType.None));

        public CurrentProductModel Model
        {
            get => (CurrentProductModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(CurrentProductModel), typeof(DemoControl), new PropertyMetadata(default(CurrentProductModel), OnModelChanged));
        
        private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var itemControl = d as DemoControl;

            var product = e.NewValue as CurrentProductModel;
            if (product != null && itemControl != null)
                product.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName.Equals("Remains"))
                        itemControl.StartYouTubeAnimation();
                    itemControl.UpdateState();
                };
        }

        public void UpdateState()
        {
            LoadingType = AppSettings.AppSettings.Current.LoadingType;

            VisualStateManager.GoToState(this, $"{LoadingType}", true);

            if (LoadingType != LoadingType.None)
            {
                if (!String.IsNullOrEmpty(Model.Name))
                    VisualStateManager.GoToState(this, "Loaded", true);

                switch (LoadingType)
                {
                    case LoadingType.FullMode:
                        VisualStateManager.GoToState(this, "ImageLoaded", true);
                        break;
                    case LoadingType.NoImageMode:
                        VisualStateManager.GoToState(this, "NoImageLoaded", true);
                        break;
                    case LoadingType.PriceMode:
                        VisualStateManager.GoToState(this, "NoImageLoaded", true);
                        break;
                }

                if (Model.DiscountPrice != 0)
                    VisualStateManager.GoToState(this, "DiscountPricesLoaded", true);
                else if (Model.Price != 0)
                    VisualStateManager.GoToState(this, "PricesLoaded", true);

                if (Model.Remains != 0)
                    VisualStateManager.GoToState(this, "RemainsLoaded", true);
                else
                    VisualStateManager.GoToState(this, "NoRemainsLoaded", true);
            }
            else
                VisualStateManager.GoToState(this, "None", true);

            if (_rootGrid != null)
                _rootGrid.Opacity = 1;
        }

        private void StartYouTubeAnimation()
        {
            Storyboard youTubeStoryboard = new Storyboard();

            youTubeStoryboard.Children.Add(this.GetDoubleAnimation(_imageBorder, 0));
            youTubeStoryboard.Children.Add(this.GetDoubleAnimation(_nameBorder, 0));
            youTubeStoryboard.Children.Add(this.GetDoubleAnimation(_priceBorder, 0));
            youTubeStoryboard.Children.Add(this.GetDoubleAnimation(_discountPriceBorder, 0));
            youTubeStoryboard.Children.Add(this.GetDoubleAnimation(_remainsBorder, 0));
            youTubeStoryboard.Children.Add(this.GetDoubleAnimation(_skuBorder, 0));

            youTubeStoryboard.Begin();
        }

        private DoubleAnimation GetDoubleAnimation(UIElement element, double to)
        {
            var animation = new DoubleAnimation
            {
                FillBehavior = FillBehavior.HoldEnd,
                Duration = TimeSpan.FromMilliseconds(300),
                From = element.Opacity,
                To = to,
                EnableDependentAnimation = true,
                AutoReverse = false
            };
            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, "Opacity");

            return animation;
        }

        public DemoControl()
        {
            this.DefaultStyleKey = typeof(DemoControl);
        }
        
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_root != null)
            {
                _root.Tapped -= RootGrid_Tapped;
                _root.RightTapped -= RootGrid_RightTapped;
            }

            if (_productTrueBuy != null)
            {
                _productTrueBuy.Tapped -= ProductTrueBuy_Tapped;
                _productTrueBuy.RightTapped -= ProductTrueBuy_RightTapped;
            }

            _root = this.GetTemplateChild("Root") as Grid;
            _productTrueBuy = this.GetTemplateChild("ProductTrueBuy") as Button;

            if (_root != null)
            {
                _root.Tapped += RootGrid_Tapped;
                _root.RightTapped += RootGrid_RightTapped;
            }

            if (_productTrueBuy != null)
            {
                _productTrueBuy.Tapped += ProductTrueBuy_Tapped;
                _productTrueBuy.RightTapped += ProductTrueBuy_RightTapped;
            }

            _rootGrid = this.GetTemplateChild("RootGrid") as Grid;
            if (_rootGrid != null)
                _rootGrid.Opacity = 0;

            _imageBorder = this.GetTemplateChild("ImageBorder") as Border;
            _nameBorder = this.GetTemplateChild("NameBorder") as Border;
            _priceBorder = this.GetTemplateChild("PriceBorder") as Border;
            _discountPriceBorder = this.GetTemplateChild("DiscountPriceBorder") as Border;
            _remainsBorder = this.GetTemplateChild("RemainsBorder") as Border;
            _skuBorder = this.GetTemplateChild("SkuBorder") as Border;
        }

        private void ProductTrueBuy_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            e.Handled = true;
            if (!Model.IsLoading && Model.Remains > 0)
                BasketInfoTapped?.Invoke(Model, e);
        }

        private void ProductTrueBuy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            if (!Model.IsLoading && Model.Remains > 0)
                BasketTapped?.Invoke(Model, e);
        }

        private void RootGrid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (!Model.IsLoading && Model.Remains > 0)
                ProductInfoTapped?.Invoke(Model, e);
        }

        private void RootGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!Model.IsLoading && Model.Remains > 0)
                ProductTapped?.Invoke(Model, e);
        }
    }
}
