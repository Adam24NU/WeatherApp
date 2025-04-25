namespace WeatherApp
{
    public class SensorMeta
    {
        public string SensorID { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public DateTime Installed { get; set; }
        public string Status { get; set; }

        public bool IsFlagged { get; set; } = false;
    }
}
