using System.Globalization;

namespace WeatherApp
{
    // This converter is used in XAML to determine visibility
    // It returns TRUE if the DateTime value is valid (not default)
    public class InstalledDateVisibilityConverter : IValueConverter
    {
        // This method is used when binding data to the UI
        // If the date is valid and not equal to default(DateTime), return true (show it)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is DateTime dt && dt != default(DateTime);

        // This is not needed in your app, so it throws an exception
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    // This is the opposite of the one above
    // It returns TRUE if the date is missing or default — useful to hide/show alternate content
    public class InvertedInstalledDateVisibilityConverter : IValueConverter
    {
        // This returns TRUE if the date is missing or default (e.g., 01/01/0001)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            !(value is DateTime dt && dt != default(DateTime));

        // Also not used in your app
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}