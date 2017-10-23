using ArtTherapy.Models.ProductModels;
using ArtTherapyCore.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ArtTherapy
{
    public class DemoControl : Control
    {
        public CurrentProductModel Model
        {
            get { return (CurrentProductModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(CurrentProductModel), typeof(DemoControl), new PropertyMetadata(default(CurrentProductModel), OnModelChanged));
        
        public Models.ProductModels.LoadingType LoadingType
        {
            get { return (Models.ProductModels.LoadingType)GetValue(VisibilityImagesProperty); }
            set { SetValue(VisibilityImagesProperty, value); }
        }

        public static readonly DependencyProperty VisibilityImagesProperty =
            DependencyProperty.Register("LoadingType", typeof(Models.ProductModels.LoadingType), typeof(DemoControl), new PropertyMetadata(Models.ProductModels.LoadingType.FullMode));

        private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var itemControl = d as DemoControl;

            var product = e.NewValue as CurrentProductModel;
            if (product != null)
            {
                if (product != null)
                    product.PropertyChanged += (sender, args) =>
                        itemControl?.UpdateState(product);
            }
        }

        public void UpdateState(CurrentProductModel model)
        {
            if (model.IsLoading)
            {
                VisualStateManager.GoToState(this, "Loading", true);
                VisualStateManager.GoToState(this, "FullMode", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Loaded", true);
                VisualStateManager.GoToState(this, $"{model.LoadingType}", true);
            }

            if (RootProgress != null)
                RootProgress.IsActive = model.IsLoading;

            VisualStateManager.GoToState(this, model.IsLoadingImage ? "ImageLoading" : "ImageLoaded", true);

            if (model.DiscountPrice != 0)
                VisualStateManager.GoToState(this, model.IsLoading ? "DiscountPricesLoading" : "DiscountPricesLoaded", true);
            else
                VisualStateManager.GoToState(this, model.IsLoadingPrice ? "PricesLoading" : "PricesLoaded", true);

            if (model.Remains != 0)
                VisualStateManager.GoToState(this, model.IsLoadingRemains ? "RemainsLoading" : "RemainsLoaded", true);
            else
                VisualStateManager.GoToState(this, model.IsLoadingRemains ? "NoRemainsLoading" : "NoRemainsLoaded", true);

            if (RootGrid != null)
            {
                switch (model.LoadingType)
                {
                    case Models.ProductModels.LoadingType.FullMode:
                        RootGrid.RowDefinitions.ElementAt(0).Height = new GridLength(110, GridUnitType.Pixel);
                        break;
                    case Models.ProductModels.LoadingType.NoImageMode:
                        RootGrid.RowDefinitions.ElementAt(0).Height = new GridLength(0, GridUnitType.Pixel);
                        break;
                    case Models.ProductModels.LoadingType.OnlyPriceMode:
                        RootGrid.RowDefinitions.ElementAt(0).Height = new GridLength(0, GridUnitType.Pixel);
                        break;
                }
            }
        }

        public Grid RootGrid { get; set; }

        public ProgressRing RootProgress { get; set; }

        public DemoControl()
        {
            this.DefaultStyleKey = typeof(DemoControl);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            RootGrid = this.GetTemplateChild("RootGrid") as Grid;
            RootProgress = this.GetTemplateChild("RootProgress") as ProgressRing;
        }
    }
}
