using ArtTherapy.Models.PostModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
