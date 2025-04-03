namespace WeatherApp.Models

{
    public class SensorMeta
    {
        public string SensorID { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime Installed { get; set; } 
        public string Status { get; set; } = string.Empty;

        public bool IsFlagged { get; set; } = false;
    }
}
