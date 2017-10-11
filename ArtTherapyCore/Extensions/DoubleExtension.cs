using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ArtTherapyCore.Extensions
{
    public static class DoubleExtension
    {
        public static double GetScrollViewProgress(this ScrollViewer value) =>
             (value.VerticalOffset + value.ActualHeight) / value.ExtentHeight;
    }
}
