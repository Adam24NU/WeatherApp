namespace WeatherApp.Models
{
    public class WeatherReading
    {
        public string Timestamp { get; set; }
        public string Temperature { get; set; }
        public string WindSpeed { get; set; }
        public string RelativeHumidity { get; set; }
        public string WindDirection { get; set; }
        
        // New: computed threshold logic
        public bool IsThresholdBreached
        {
            get
            {
                double.TryParse(Temperature, out var temp);
                double.TryParse(WindSpeed, out var wind);
                double.TryParse(RelativeHumidity, out var humidity);

                return temp > 40 || wind > 50 || humidity > 90 || humidity < 20;
            }
        }

    }
}
