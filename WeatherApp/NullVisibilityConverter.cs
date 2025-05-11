using System.Globalization;

namespace WeatherApp
{
    // This converter is used to control visibility in XAML based on whether a value is null or empty.
    public class NullVisibilityConverter : IValueConverter
    {
        // Convert method: returns true if the value is not null or empty, false otherwise.
        // In MAUI, true typically means "Visible" and false means "Collapsed" or "Hidden".
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If value is null or empty string, return false (don't show it)
            // Otherwise, return true (show it)
            return value == null || string.IsNullOrEmpty(value.ToString()) ? false : true;
        }

        // ConvertBack is not needed — so this throws a NotImplementedException
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}