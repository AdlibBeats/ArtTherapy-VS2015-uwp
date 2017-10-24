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
            get { return (Models.ProductModels.LoadingType)GetValue(LoadingTypeProperty); }
            set { SetValue(LoadingTypeProperty, value); }
        }

        public static readonly DependencyProperty LoadingTypeProperty =
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
            LoadingType = model.LoadingType;

            VisualStateManager.GoToState(this, $"{LoadingType}", true);

            VisualStateManager.GoToState(this, model.IsLoading ? "Loading" : "Loaded", true);

            if (RootProgress != null)
                RootProgress.IsActive = model.IsLoading;

            if (model.IsLoading)
                VisualStateManager.GoToState(this, "ImageLoading", true);
            else
            {
                switch (LoadingType)
                {
                    case Models.ProductModels.LoadingType.FullMode:
                        VisualStateManager.GoToState(this, "ImageLoaded", true);
                        break;
                    case Models.ProductModels.LoadingType.NoImageMode:
                        VisualStateManager.GoToState(this, "NoImageLoaded", true);
                        break;
                    case Models.ProductModels.LoadingType.OnlyPriceMode:
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

        public ProgressRing RootProgress { get; set; }

        public DemoControl()
        {
            this.DefaultStyleKey = typeof(DemoControl);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            RootProgress = this.GetTemplateChild("RootProgress") as ProgressRing;
        }
    }
}
