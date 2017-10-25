using ArtTherapy.Models.ProductModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
        
        public LoadingType LoadingType
        {
            get { return (LoadingType)GetValue(LoadingTypeProperty); }
            private set { SetValue(LoadingTypeProperty, value); }
        }

        public static readonly DependencyProperty LoadingTypeProperty =
            DependencyProperty.Register("LoadingType", typeof(LoadingType), typeof(DemoControl), new PropertyMetadata(LoadingType.FullMode));

        private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var itemControl = d as DemoControl;

            var product = e.NewValue as CurrentProductModel;
            if (product != null && itemControl != null)
            {
                if (product != null)
                    product.PropertyChanged += (sender, args) =>
                        itemControl.UpdateState(product);
            }
        }

        public void UpdateState(CurrentProductModel model)
        {
            LoadingType = model.LoadingType;

            VisualStateManager.GoToState(this, $"{LoadingType}", true);

            if (LoadingType != LoadingType.None)
            {
                if (RootProgress != null)
                    RootProgress.IsActive = model.IsLoading;

                VisualStateManager.GoToState(this, model.IsLoading ? "Loading" : "Loaded", true);

                if (model.IsLoading)
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
                        case LoadingType.OnlyPriceMode:
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
            {
                RootProgress.IsActive = false;
                VisualStateManager.GoToState(this, "None", true);
            }
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
