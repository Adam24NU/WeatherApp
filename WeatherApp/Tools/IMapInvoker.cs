using WeatherApp.Core.ViewModels;

namespace WeatherApp.Tools
{
    public interface IMapInvoker
    {
        Task UpdateMapLocationAsync(double latitude, double longitude, string sensorType, IEnumerable<MapSensorDisplay> sensorDisplays);
    }

}
