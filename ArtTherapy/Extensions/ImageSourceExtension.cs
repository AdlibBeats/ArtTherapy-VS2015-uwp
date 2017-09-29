using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ArtTherapy.Extensions
{
    public enum DefaultImage
    {
        Icon,
        Picture,
        Avatar
    }

    public static class ImageSourceExtension
    {
        public static ImageSource GetImageSource(this string value, DefaultImage defaultImage = DefaultImage.Avatar)
        {
            string defaultUriPath = "ms-appx:///Assets/Account Icon.png";

            switch (defaultImage)
            {
                case DefaultImage.Icon: defaultUriPath = "ms-appx:///Assets/Account Icon.png"; break;
                case DefaultImage.Picture: defaultUriPath = "ms-appx:///Assets/Account Icon.png"; break;
                case DefaultImage.Avatar: defaultUriPath = "ms-appx:///Assets/Account Icon.png"; break;
            }

            try
            {
                BitmapImage imageSource = !String.IsNullOrEmpty(value) ?
                    new BitmapImage()
                    {
                        CreateOptions = BitmapCreateOptions.IgnoreImageCache,
                        UriSource = new Uri(value, UriKind.RelativeOrAbsolute)
                    } :
                    new BitmapImage()
                    {
                        CreateOptions = BitmapCreateOptions.IgnoreImageCache,
                        UriSource = new Uri(defaultUriPath, UriKind.RelativeOrAbsolute)
                    };
                return imageSource;
            }
            catch
            {
                Debug.WriteLine("Изображение не удалось установить.");

                BitmapImage imageSource = new BitmapImage()
                {
                    CreateOptions = BitmapCreateOptions.IgnoreImageCache,
                    UriSource = new Uri(defaultUriPath, UriKind.RelativeOrAbsolute)
                };
                return imageSource;
            }
        }
    }
}
