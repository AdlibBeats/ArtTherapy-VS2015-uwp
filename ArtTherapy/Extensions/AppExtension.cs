using System;
using System.Diagnostics;
using ArtTherapy.Pages.AboutAppPages;
using ArtTherapy.Pages.MenuPages;
using ArtTherapy.Pages.PostPages;
using ArtTherapy.Pages.ProfilePages;
using ArtTherapy.Pages.SettingsPages;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ArtTherapy.Extensions
{
    public static class AppExtension
    {
        public static bool IsMobile =>
            ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0);
    }

    public class FrameExtension
    {
        public static readonly DependencyProperty PageStringProperty =
           DependencyProperty.RegisterAttached("PageString", typeof(string), typeof(FrameExtension), new PropertyMetadata("", OnPageStringChanged));

        public static string GetPageString(DependencyObject obj) { return (string)obj.GetValue(PageStringProperty); }
        public static void SetPageString(DependencyObject obj, string value) { obj.SetValue(PageStringProperty, value); }
        private static void OnPageStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frame = d as Frame;
            if (d != null)
            {
                switch ((string)e.NewValue)
                {
                    case "AboutAppPage":
                        frame.Navigate(typeof(AboutAppPage));
                        break;
                    case "MenuPage":
                        frame.Navigate(typeof(MenuPage));
                        break;
                    case "CurrentPostPage":
                        frame.Navigate(typeof(CurrentPostPage));
                        break;
                    case "PostPage":
                        frame.Navigate(typeof(PostPage));
                        break;
                    case "ProfilePage":
                        frame.Navigate(typeof(ProfilePage));
                        break;
                    case "SettingsPage":
                        frame.Navigate(typeof(SettingsPage));
                        break;
                    default:
                        Debug.WriteLine("Frame: неверно указано название страницы");
                        break;
                }
                frame.BackStack.Clear();
                frame.ForwardStack.Clear();
            }
        }
    }
}
