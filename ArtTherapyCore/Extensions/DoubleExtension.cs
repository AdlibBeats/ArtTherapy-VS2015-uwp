using Windows.UI.Xaml.Controls;

namespace ArtTherapyCore.Extensions
{
    public static class DoubleExtension
    {
        public static double GetScrollViewProgress(this ScrollViewer value) =>
             (value.VerticalOffset + value.ActualHeight) / value.ExtentHeight;
    }
}
