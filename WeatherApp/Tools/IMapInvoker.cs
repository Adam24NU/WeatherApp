namespace WeatherApp.Tools
{
    public interface IMapInvoker
    {
        Task UpdateMapLocationAsync(double latitude, double longitude);
    }
}
