namespace WeatherApp.Core.Tools
{
    public interface INavigationService
    {
        Task NavigateToAsync(string pageName);
    }
}
