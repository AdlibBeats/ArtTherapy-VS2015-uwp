using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ArtTherapyCore.ValueConverters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) =>
            ((bool)value) ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            ((Visibility)value == Visibility.Visible) ? true : false;
    }

    public class VisibilityInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) =>
            ((bool)value) ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            (value is Visibility.Visible) ? false : true;
    }
}
