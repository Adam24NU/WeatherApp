namespace WeatherApp.Tools
{
    public interface INavigationService
    {
        Task NavigateToAsync(string pageName);
    }
}
