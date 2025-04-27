namespace WeatherApp.Tools
{
    public interface IAlertService
    {
        Task DisplayAlert(string title, string message, string cancel);
    }

    public class AlertService : IAlertService
    {
        public Task DisplayAlert(string title, string message, string cancel)
        {
            return Shell.Current.DisplayAlert(title, message, cancel);
        }
    }
}
