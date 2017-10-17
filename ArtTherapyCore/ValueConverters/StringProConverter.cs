using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ArtTherapyCore.ValueConverters
{
    public class StringProConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) =>
            (String.IsNullOrEmpty(value as String)) ? "Загрузка..." : value;

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            (!String.IsNullOrEmpty(value as String)) ? String.Empty : value;
    }
}
