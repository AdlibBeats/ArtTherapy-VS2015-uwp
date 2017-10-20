using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ArtTherapyCore.ValueConverters
{
    public class RemainsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) =>
            (uint)value == 0 ? String.Empty : $"{value} шт.";

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            String.IsNullOrEmpty(value as String) ? 0 : value;
    }
}
