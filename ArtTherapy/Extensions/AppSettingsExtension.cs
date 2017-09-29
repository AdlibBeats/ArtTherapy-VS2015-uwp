using System;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Phone.UI.Input;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace ArtTherapy.Extensions
{
    public abstract class AppSettingsExtension
    {
        protected Frame RootFrame { get; set; }

        public AppSettingsExtension(Frame rootFrame)
        {
            RootFrame = rootFrame;
        }

        public abstract AppSettings Create();
    }

    public sealed class MobileSettingsExtension : AppSettingsExtension
    {
        public MobileSettingsExtension(Frame rootFrame) : base(rootFrame)
        {

        }

        public override AppSettings Create() => new MobileSettings(RootFrame);
    }

    public sealed class DesktopSettingsExtension : AppSettingsExtension
    {
        public DesktopSettingsExtension(Frame rootFrame) : base(rootFrame)
        {

        }

        public override AppSettings Create() => new DesktopSettings(RootFrame);
    }

    public abstract class AppSettings
    {
        protected Frame RootFrame { get; set; }

        public AppSettings(Frame rootFrame)
        {
            RootFrame = rootFrame;

            // Установка фона фрейма
            RootFrame.Background = new SolidColorBrush(Colors.Orange);

            // Установка минимального размера окна
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(300, 300));
        }

        public abstract bool SetSettings();
    }

    sealed class MobileSettings : AppSettings
    {
        EventHandler<BackPressedEventArgs> handler;

        public MobileSettings(Frame rootFrame) : base(rootFrame)
        {

        }

        public override bool SetSettings()
        {
            if (handler == null)
                handler = new EventHandler<BackPressedEventArgs>(HardwareButtons_BackPressed);

            var statusBar = StatusBar.GetForCurrentView() as StatusBar;
            if (statusBar != null)
            {
                statusBar.ForegroundColor = Colors.Black;
                statusBar.BackgroundColor = Colors.Orange;
                statusBar.BackgroundOpacity = 1;
            }

            return true;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            if (RootFrame.CanGoBack)
                RootFrame.GoBack();
        }
    }

    sealed class DesktopSettings : AppSettings
    {
        KeyEventHandler handler;

        public DesktopSettings(Frame rootFrame) : base(rootFrame)
        {

        }

        public override bool SetSettings()
        {
            if (handler == null)
                handler = new KeyEventHandler(RootFrame_KeyUp);

            var titleBar = ApplicationView.GetForCurrentView().TitleBar as ApplicationViewTitleBar;
            if (titleBar != null)
            {
                titleBar.ButtonHoverBackgroundColor = Colors.Gray;
                titleBar.ButtonHoverForegroundColor = Colors.Black;
                titleBar.ButtonBackgroundColor = Colors.Orange;
                titleBar.ButtonForegroundColor = Colors.Black;
                titleBar.BackgroundColor = Colors.Orange;
                titleBar.ForegroundColor = Colors.Black;
            }

            return true;
        }

        private void RootFrame_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape)
                if (RootFrame.CanGoBack)
                    RootFrame.GoBack();
        }
    }
}
