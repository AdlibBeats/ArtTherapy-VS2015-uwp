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
    public enum VisibilityImages : byte
    {
        Visible,
        Collapsed
    }

    public class DemoControl : Control
    {
        public CurrentProductModel Model
        {
            get { return (CurrentProductModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(CurrentProductModel), typeof(DemoControl), new PropertyMetadata(default(CurrentProductModel), OnModelChanged));
        
        public VisibilityImages VisibilityImages
        {
            get { return (VisibilityImages)GetValue(VisibilityImagesProperty); }
            set { SetValue(VisibilityImagesProperty, value); }
        }

        public static readonly DependencyProperty VisibilityImagesProperty =
            DependencyProperty.Register("VisibilityImages", typeof(VisibilityImages), typeof(DemoControl), new PropertyMetadata(VisibilityImages.Visible));
        
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
            VisualStateManager.GoToState(this, model.IsLoading ? "Loading" : "Loaded", true);

            if (RootProgress != null)
                RootProgress.IsActive = model.IsLoading;

            //if (Model.IsLoadingImage)
            //    VisualStateManager.GoToState(this, "ImageLoading", true);
            //else
            //{
            //    VisualStateManager.GoToState(this, "ImageLoaded", true);

            //    if (RootGrid != null)
            //    {
            //        //if (VisibilityImages == VisibilityImages.Visible)
            //        //    RootGrid.RowDefinitions.ElementAt(0).Height = new GridLength(1, GridUnitType.Star);
            //        //else
            //        //    RootGrid.RowDefinitions.ElementAt(0).Height = new GridLength(0, GridUnitType.Pixel);
            //        //if (String.IsNullOrEmpty(model.ImageUrl))
            //        //    RootGrid.RowDefinitions.ElementAt(0).Height = new GridLength(0, GridUnitType.Pixel);
            //        //else
            //        //    RootGrid.RowDefinitions.ElementAt(0).Height = new GridLength(1, GridUnitType.Star);
            //    }
            //}

            VisualStateManager.GoToState(this, model.IsLoadingImage ? "ImageLoading" : "ImageLoaded", true);

            if (model.DiscountPrice != 0)
                VisualStateManager.GoToState(this, model.IsLoading ? "DiscountPricesLoading" : "DiscountPricesLoaded", true);
            else
                VisualStateManager.GoToState(this, model.IsLoadingPrice ? "PricesLoading" : "PricesLoaded", true);

            if (model.Remains != 0)
                VisualStateManager.GoToState(this, model.IsLoadingRemains ? "RemainsLoading" : "RemainsLoaded", true);
            else
                VisualStateManager.GoToState(this, model.IsLoadingRemains ? "NoRemainsLoading" : "NoRemainsLoaded", true);
        }

        public Grid RootGrid { get; set; }

        public ProgressRing RootProgress { get; set; }

        public DemoControl()
        {
            this.DefaultStyleKey = typeof(DemoControl);
            Model = new CurrentProductModel();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            RootGrid = this.GetTemplateChild("RootGrid") as Grid;
            RootProgress = this.GetTemplateChild("RootProgress") as ProgressRing;

            //if (RootGrid != null)
            //{
            //    if (VisibilityImages == VisibilityImages.Visible)
            //        RootGrid.RowDefinitions.ElementAt(0).Height = new GridLength(1, GridUnitType.Star);
            //    else
            //        RootGrid.RowDefinitions.ElementAt(0).Height = new GridLength(0, GridUnitType.Pixel);
            //}
        }
    }
}
