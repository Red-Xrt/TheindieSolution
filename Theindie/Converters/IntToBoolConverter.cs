using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Theindie.Converters
{
    // Converter so sánh: Nếu Value == Parameter thì trả về True
    public class IntToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int currentPage && parameter is string targetPageStr && int.TryParse(targetPageStr, out int targetPage))
            {
                return currentPage == targetPage;
            }
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}