namespace WeatherApp.Services
{
    public interface IStorageService
    {
        long GetTotalStorageBytes();
        long GetAvailableStorageBytes();
    }
}