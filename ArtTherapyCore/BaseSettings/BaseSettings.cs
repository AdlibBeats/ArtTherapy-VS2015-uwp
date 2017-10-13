using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ArtTherapyCore.BaseSettings
{
    public abstract class BaseFactorySettings
    {
        public BaseFactorySettings()
        {
        }

        public abstract BaseSettings Create(Frame frame);
    }

    public abstract class BaseSettings
    {
        protected Frame RootFrame { get; set; }

        public BaseSettings()
        {
            SetPreferredMinSize();
        }

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
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(400, 400));
        }

        /// <summary>
        /// Установка настроек.
        /// </summary>
        /// <returns></returns>
        public abstract bool SetSettings();
    }
}
