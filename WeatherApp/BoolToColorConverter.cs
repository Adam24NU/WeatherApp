using Microsoft.Maui.Controls;     // For UI-related interfaces and types
using System;
using System.Globalization;

namespace WeatherApp.Converters
{
    // This class converts a boolean value to a color for UI display
    // Used in XAML to highlight items with red if a condition (like threshold breach) is true
    public class BoolToColorConverter : IValueConverter
    {
        // Called automatically during UI binding to convert a value into a color
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If value is a boolean and true (alert condition), return red
            if (value is bool isAlert && isAlert)
                return Colors.IndianRed;

            // Otherwise, return white as default background
            return Colors.White;
        }

        // ConvertBack is not used in this scenario, so we throw a NotImplementedException
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}