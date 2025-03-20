using Avalonia.Data.Converters;
using Avalonia.Media;
using System;

namespace GetStartedApp.Converters
{
    public class BoolToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool boolValue && boolValue)
            {
                return new SolidColorBrush(Avalonia.Media.Colors.Orange);
            }
            return new SolidColorBrush(Avalonia.Media.Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}