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

namespace ArtTherapy
{
    public class DemoControl : Control
    {
        public event TappedEventHandler ProductTapped;
        public event RightTappedEventHandler ProductInfoTapped;
        public event TappedEventHandler BasketTapped;
        public event RightTappedEventHandler BasketInfoTapped;

        public LoadingType LoadingType
        {
            get { return (LoadingType)GetValue(LoadingTypeProperty); }
            set { SetValue(LoadingTypeProperty, value); }
        }

        public static readonly DependencyProperty LoadingTypeProperty =
            DependencyProperty.Register("LoadingType", typeof(LoadingType), typeof(DemoControl), new PropertyMetadata(LoadingType.None));

        public CurrentProductModel Model
        {
            get { return (CurrentProductModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(CurrentProductModel), typeof(DemoControl), new PropertyMetadata(default(CurrentProductModel), OnModelChanged));
        
        private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var itemControl = d as DemoControl;

            var product = e.NewValue as CurrentProductModel;
            if (product != null && itemControl != null)
                product.PropertyChanged += (sender, args) =>
                itemControl.UpdateState();
        }

        public void UpdateState()
        {
            LoadingType = AppSettings.AppSettings.Current.LoadingType;

            VisualStateManager.GoToState(this, $"{LoadingType}", true);

            if (LoadingType != LoadingType.None)
            {
                VisualStateManager.GoToState(this, Model.IsLoading ? "Loading" : "Loaded", true);

                if (Model.IsLoading)
                    VisualStateManager.GoToState(this, "ImageLoading", true);
                else
                {
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
                }

                if (Model.DiscountPrice != 0)
                    VisualStateManager.GoToState(this, Model.IsLoading ? "DiscountPricesLoading" : "DiscountPricesLoaded", true);
                else
                    VisualStateManager.GoToState(this, Model.IsLoadingPrice ? "PricesLoading" : "PricesLoaded", true);

                if (Model.Remains != 0)
                    VisualStateManager.GoToState(this, Model.IsLoadingRemains ? "RemainsLoading" : "RemainsLoaded", true);
                else
                    VisualStateManager.GoToState(this, Model.IsLoadingRemains ? "NoRemainsLoading" : "NoRemainsLoaded", true);
            }
            else
                VisualStateManager.GoToState(this, "None", true);
        }

        private Grid Root { get; set; }

        private Button ProductTrueBuy { get; set; }

        public DemoControl()
        {
            this.DefaultStyleKey = typeof(DemoControl);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            Root = this.GetTemplateChild("Root") as Grid;
            ProductTrueBuy = this.GetTemplateChild("ProductTrueBuy") as Button;

            if (Root != null)
            {
                Root.Tapped += RootGrid_Tapped;
                Root.RightTapped += RootGrid_RightTapped;
            }

            if (ProductTrueBuy != null)
            {
                ProductTrueBuy.Tapped += ProductTrueBuy_Tapped;
                ProductTrueBuy.RightTapped += ProductTrueBuy_RightTapped;
            }
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
