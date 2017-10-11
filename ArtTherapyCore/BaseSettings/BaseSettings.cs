using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ArtTherapyCore.BaseSettings
{
    public abstract class BaseFactorySettings
    {
        protected Frame RootFrame { get; set; }

        public BaseFactorySettings(Frame rootFrame)
        {
            RootFrame = rootFrame;
        }

        public abstract BaseSettings Create();
    }

    public abstract class BaseSettings
    {
        protected Frame RootFrame { get; set; }

        public BaseSettings(Frame rootFrame)
        {
            RootFrame = rootFrame;

            SetBackgroundColor();

            SetPreferredMinSize();
        }

        /// <summary>
        /// Установка фона фрейма.
        /// </summary>
        /// <returns></returns>
        protected virtual bool SetBackgroundColor()
        {
            RootFrame.Background = new SolidColorBrush(Colors.White);
            return true;
        }

        /// <summary>
        /// Установка минимального размера окна.
        /// </summary>
        protected virtual void SetPreferredMinSize()
        {
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(300, 300));
        }

        /// <summary>
        /// Установка настроек.
        /// </summary>
        /// <returns></returns>
        public abstract bool SetSettings();
    }
}
