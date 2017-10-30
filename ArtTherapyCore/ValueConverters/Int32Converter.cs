using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ArtTherapyCore.ValueConverters
{
    public class Int32Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) =>
            (int?)value == null || (int?)value == 0 ? String.Empty : value;

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            String.IsNullOrEmpty(value as String) ? 0 : int.Parse(value as string);
    }
}
