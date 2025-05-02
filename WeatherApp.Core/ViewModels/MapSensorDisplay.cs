namespace WeatherApp.Core.ViewModels;

public class MapSensorDisplay
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string SensorName { get; set; }
    public string QuantityName { get; set; }
    public double? Value { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public string Unit { get; set; }
    public string Type { get; set; }
}
