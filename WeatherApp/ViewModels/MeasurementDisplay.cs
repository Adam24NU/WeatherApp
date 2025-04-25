namespace WeatherApp.ViewModels
{
    // Data transfer object (DTO) class
    public class MeasurementDisplay
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public double? Value { get; set; }
        public string Unit { get; set; }
    }
}
