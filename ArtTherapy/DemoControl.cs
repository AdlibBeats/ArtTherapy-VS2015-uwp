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
                    itemControl.UpdateState(product);
        }

        public void UpdateLoadingType(LoadingType loadingType)
        {
            VisualStateManager.GoToState(this, $"{loadingType}", true);

            switch (loadingType)
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

        public void UpdateState(CurrentProductModel model)
        {
            VisualStateManager.GoToState(this, $"{model.LoadingType}", true);

            if (model.LoadingType != LoadingType.None)
            {
                VisualStateManager.GoToState(this, model.IsLoading ? "Loading" : "Loaded", true);

                if (model.IsLoading)
                    VisualStateManager.GoToState(this, "ImageLoading", true);
                else
                {
                    switch (model.LoadingType)
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

                if (model.DiscountPrice != 0)
                    VisualStateManager.GoToState(this, model.IsLoading ? "DiscountPricesLoading" : "DiscountPricesLoaded", true);
                else
                    VisualStateManager.GoToState(this, model.IsLoadingPrice ? "PricesLoading" : "PricesLoaded", true);

                if (model.Remains != 0)
                    VisualStateManager.GoToState(this, model.IsLoadingRemains ? "RemainsLoading" : "RemainsLoaded", true);
                else
                    VisualStateManager.GoToState(this, model.IsLoadingRemains ? "NoRemainsLoading" : "NoRemainsLoaded", true);
            }
            else
                VisualStateManager.GoToState(this, "None", true);
        }

        private Grid RootGrid { get; set; }

        private Button ProductTrueBuy { get; set; }

        public DemoControl()
        {
            this.DefaultStyleKey = typeof(DemoControl);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            RootGrid = this.GetTemplateChild("RootGrid") as Grid;
            ProductTrueBuy = this.GetTemplateChild("ProductTrueBuy") as Button;

            if (RootGrid != null)
            {
                RootGrid.Tapped += RootGrid_Tapped;
                RootGrid.RightTapped += RootGrid_RightTapped;
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
