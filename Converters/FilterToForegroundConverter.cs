using Avalonia.Data.Converters;
using Avalonia.Media;
using System;

namespace GetStartedApp.Converters
{
    public class FilterToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string selectedFilter = value as string;
            string currentFilter = parameter as string;

            if (selectedFilter == currentFilter)
            {
                return new SolidColorBrush(Colors.Orange);
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}