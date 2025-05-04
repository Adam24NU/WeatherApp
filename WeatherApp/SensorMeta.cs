namespace WeatherApp
{
    public class SensorMeta
    {
        public int SensorId { get; set; }
        public string SensorType { get; set; }
        public string MeasurementFrequency { get; set; }
        public string SafeLevel { get; set; }
        public string Status { get; set; }
        public bool IsFlagged { get; set; } = false;
        
        // addedd for Verification data for manager task Bart
        public double LastReading { get; set; }
        
        public DateTime Timestamp { get; set; }
        
        public bool IsDataValid { get; set; }
        
        public string DataQualityComment { get; set; }
      

    }

}