using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ArtTherapy
{
    public class SuperItem : GridViewItem
    {


        public bool IsLoaded
        {
            get { return (bool)GetValue(IsLoadedProperty); }
            set { SetValue(IsLoadedProperty, value); }
        }
        
        public static readonly DependencyProperty IsLoadedProperty =
            DependencyProperty.Register("IsLoaded", typeof(bool), typeof(SuperItem), new PropertyMetadata(true));



        public SuperItem()
        {
            this.DefaultStyleKey = typeof(SuperItem);
        }



        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
