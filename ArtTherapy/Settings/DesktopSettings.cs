using ArtTherapyCore.BaseSettings;
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

namespace ArtTherapy.Settings
{
    public sealed class DesktopFactorySettings : BaseFactorySettings
    {
        public DesktopFactorySettings(Frame rootFrame) : base(rootFrame)
        {

        }

        public override BaseSettings Create() => new DesktopSettings(RootFrame);
    }

    public sealed class DesktopSettings : BaseSettings
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
                titleBar.ButtonBackgroundColor = new Color() { R = 240, G = 60, B = 60, A = 255 };
                titleBar.ButtonForegroundColor = Colors.Black;
                titleBar.BackgroundColor = new Color() { R = 240, G = 60, B = 60, A = 255 };
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
