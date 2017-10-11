﻿using ArtTherapyCore.BaseSettings;
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
    public sealed class PhoneFactorySettings : BaseFactorySettings
    {
        public PhoneFactorySettings(Frame rootFrame) : base(rootFrame)
        {

        }

        public override BaseSettings Create() => new PhoneSettings(RootFrame);
    }

    public sealed class PhoneSettings : BaseSettings
    {
        EventHandler<BackPressedEventArgs> handler;

        public PhoneSettings(Frame rootFrame) : base(rootFrame)
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
