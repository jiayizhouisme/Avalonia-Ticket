using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace GetStartedApp.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected && isSelected)
            {
                return new SolidColorBrush(Color.Parse("#f4e8d3")); // 选中
            }
            //return new SolidColorBrush(Color.Parse("#F0F8FF")); // 未选中
            return new SolidColorBrush(Color.Parse("#F5EFE8"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}