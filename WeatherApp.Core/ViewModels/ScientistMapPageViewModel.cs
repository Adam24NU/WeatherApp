using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using WeatherApp.Core.Repositories;

namespace WeatherApp.Core.ViewModels;

public partial class ScientistMapPageViewModel
{
    private readonly ISiteRepository _siteRepository;
    private readonly MeasurementRepository _measurementRepository;
    private readonly SensorRepository _sensorRepository;
    private readonly PhysicalQuantityRepository _physicalQuantitiesRepository;

    // Constructor  
    public ScientistMapPageViewModel(ISiteRepository siteRepository, DatabaseConnection dbConnection)
    {
        _siteRepository = siteRepository;
        _measurementRepository = new MeasurementRepository(dbConnection);
        _sensorRepository = new SensorRepository(dbConnection);
        _physicalQuantitiesRepository = new PhysicalQuantityRepository(dbConnection);
        ShowSensorCommand = new AsyncRelayCommand<string>(UpdateMapData);
    }

    // Command to show sensor by type  
    public IAsyncRelayCommand<string> ShowSensorCommand { get; }

    public async Task UpdateMapData(string type)
    {
        Debug.WriteLine($"[UpdateMapData] Starting map update for type: {type}");

        var site = await _siteRepository.GetSiteByTypeAsync(type);
        if (site == null)
        {
            Debug.WriteLine($"[UpdateMapData] No site found for type: {type}");
            return;
        }
        Debug.WriteLine($"[UpdateMapData] Found site: ID={site.SiteId}, Lat={site.Latitude}, Lon={site.Longitude}");

        Debug.WriteLine($"[UpdateMapData] _sensorRepository is null: {_sensorRepository == null}");

        var sensorIds = await _sensorRepository.GetSensorIDsBySiteIdAsync(site.SiteId);
        if (sensorIds == null || !sensorIds.Any())
        {
            Debug.WriteLine($"[UpdateMapData] No sensors found for site ID: {site.SiteId}");
            return;
        }
        Debug.WriteLine($"[UpdateMapData] Found {sensorIds.Count} sensor IDs");

        foreach (var sensorId in sensorIds)
        {
            Debug.WriteLine($"[UpdateMapData] Fetching sensor with ID: {sensorId}");
            var sensor = await _sensorRepository.GetSensorByIdAsync(sensorId);
            if (sensor == null)
            {
                Debug.WriteLine($"[UpdateMapData] No sensor found for sensor ID: {sensorId}");
                continue;
            }
            Debug.WriteLine($"[UpdateMapData] Found sensor: Name={sensor.Name}, ID={sensor.SensorId}");

            var quantities = await _physicalQuantitiesRepository.GetPhysicalQuantitiesBySensorIdAsync(sensor.SensorId);
            if (quantities == null || !quantities.Any())
            {
                Debug.WriteLine($"[UpdateMapData] No physical quantities found for sensor ID: {sensor.SensorId}");
                continue;
            }
            Debug.WriteLine($"[UpdateMapData] Found {quantities.Count} physical quantities for sensor ID: {sensor.SensorId}");

            foreach (var quantity in quantities)
            {
                Debug.WriteLine($"[UpdateMapData] Fetching measurement for quantity ID: {quantity.QuantityId}");
                var measurement = await _measurementRepository.GetMeasurementByQuantityIdAsync(quantity.QuantityId);
                if (measurement == null)
                {
                    Debug.WriteLine($"[UpdateMapData] No measurement found for quantity ID: {quantity.QuantityId}");
                    continue;
                }
                Debug.WriteLine($"[UpdateMapData] Measurement found: Value={measurement.Value}, Date={measurement.Date}, Time={measurement.Time}");

                var mapData = new MapSensorDisplay
                {
                    Latitude = site.Latitude,
                    Longitude = site.Longitude,
                    SensorName = sensor.Name,
                    QuantityName = quantity.QuantityName,
                    Unit = quantity.Unit,
                    Value = measurement.Value,
                    Date = measurement.Date,
                    Time = measurement.Time,
                    Type = sensor.SensorType
                };

                Debug.WriteLine($"[UpdateMapData] Triggering map update event with sensor data for sensor ID: {sensor.SensorId}");
                OnMapLocationUpdated?.Invoke(site.Latitude, site.Longitude, type, new List<MapSensorDisplay> { mapData });
            }
        }

        Debug.WriteLine("[UpdateMapData] Finished processing all sensor data.");
    }

    // Update the event to include sensor type
    public event Func<double, double, string, IEnumerable<MapSensorDisplay>, Task> OnMapLocationUpdated;

}
