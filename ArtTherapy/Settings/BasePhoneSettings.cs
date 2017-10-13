using ArtTherapyCore.BaseSettings;
using System;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace ArtTherapy.Settings
{
    public sealed class PhoneFactorySettings : BaseFactorySettings
    {
        public PhoneFactorySettings() : base()
        {

        }

        public override BaseSettings Create(Frame frame) => new BasePhoneSettings(frame);
    }

    public sealed class BasePhoneSettings : BaseSettings
    {
        EventHandler<BackPressedEventArgs> handler;

        public BasePhoneSettings(Frame rootFrame) : base(rootFrame)
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
                statusBar.BackgroundColor = new Color() { R = 240, G = 60, B = 60, A = 255 };
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
}
