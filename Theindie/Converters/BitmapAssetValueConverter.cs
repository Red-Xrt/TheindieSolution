using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;

namespace Theindie.Converters
{
    public class BitmapAssetValueConverter : IValueConverter
    {
        public static BitmapAssetValueConverter Instance = new BitmapAssetValueConverter();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string rawUri && !string.IsNullOrEmpty(rawUri))
            {
                try
                {
                    Uri uri = rawUri.StartsWith("/") ? new Uri($"avares://Theindie{rawUri}") : new Uri(rawUri);
                    return new Bitmap(AssetLoader.Open(uri));
                }
                catch { return null; }
            }
            return null;
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}