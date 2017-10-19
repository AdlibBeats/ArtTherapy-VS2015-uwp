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

        public void UpdateState(CurrentProductModel currentPostModel)
        {
            VisualStateManager.GoToState(this, Model.IsLoading ? "Loading" : "Loaded", true);

            //Debug.WriteLine(currentPostModel.CanLoadingImage);

            //if (currentPostModel.CanLoadingImage)
            VisualStateManager.GoToState(this, Model.IsLoadingImage ? "ImageLoading" : "ImageLoaded", true);
            //else
                //VisualStateManager.GoToState(this, "NoImageLoaded", true);

            if (currentPostModel.DiscountPrice != 0)
                VisualStateManager.GoToState(this, Model.IsLoading ? "DiscountPricesLoading" : "DiscountPricesLoaded", true);
            else
                VisualStateManager.GoToState(this, Model.IsLoadingPrice ? "PricesLoading" : "PricesLoaded", true);
            if (currentPostModel.Remains != 0)
                VisualStateManager.GoToState(this, Model.IsLoadingRemains ? "RemainsLoading" : "RemainsLoaded", true);
            else
                VisualStateManager.GoToState(this, Model.IsLoadingRemains ? "NoRemainsLoading" : "NoRemainsLoaded", true);
        }

        public DemoControl()
        {
            this.DefaultStyleKey = typeof(DemoControl);
            Model = new CurrentProductModel();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
