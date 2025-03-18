using Avalonia.Data.Converters;
using Avalonia;
using System;
using System.Globalization;

namespace GetStartedApp.Converters
{
    public class IntToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int marginValue)
            {
                return new Thickness(0, 0, marginValue, 0);
            }
            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}