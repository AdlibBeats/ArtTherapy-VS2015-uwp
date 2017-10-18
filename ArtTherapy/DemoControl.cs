﻿using ArtTherapy.Models.PostModels;
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
        public CurrentPostModel Model
        {
            get { return (CurrentPostModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(CurrentPostModel), typeof(DemoControl), new PropertyMetadata(default(CurrentPostModel), OnModelChanged));

        private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var itemControl = d as DemoControl;

            var product = e.NewValue as CurrentPostModel;
            if (product != null)
            {
                if (product != null)
                    product.PropertyChanged += (sender, args) =>
                        itemControl?.UpdateState(product);
            }
        }

        public void UpdateState(CurrentPostModel currentPostModel)
        {
            VisualStateManager.GoToState(this, Model.IsLoading ? "Loading" : "Loaded", true);
            VisualStateManager.GoToState(this, Model.IsLoadingImage ? "ImageLoading" : "ImageLoaded", true);
            if (currentPostModel.DiscountDescription != null)
                VisualStateManager.GoToState(this, Model.IsLoading ? "DiscountPricesLoading" : "DiscountPricesLoaded", true);
            else
                VisualStateManager.GoToState(this, Model.IsLoadingPrices ? "PricesLoading" : "PricesLoaded", true);
            VisualStateManager.GoToState(this, Model.IsLoadingRemains ? "RemainsLoading" : "RemainsLoaded", true);
        }

        public DemoControl()
        {
            this.DefaultStyleKey = typeof(DemoControl);
            Model = new CurrentPostModel();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
