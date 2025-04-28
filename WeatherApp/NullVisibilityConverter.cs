
using System.Globalization;


namespace WeatherApp
{
    public class NullVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is null or an empty string, and return Visibility.Collapsed if true
            return value == null || string.IsNullOrEmpty(value.ToString()) ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}