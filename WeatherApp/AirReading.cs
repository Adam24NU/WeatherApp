namespace WeatherApp.Models
{
    public class AirReading
    {
        public string Timestamp { get; set; }
        public string NO2 { get; set; }
        public string PM25 { get; set; }
        public string PM10 { get; set; }
        public string Su { get; set; }

        // ⚠️ New computed property
        public bool IsThresholdBreached
        {
            get
            {
                double.TryParse(NO2, out var no2Val);
                double.TryParse(PM25, out var pm25Val);
                return no2Val > 40 || pm25Val > 25;
            }
        }

        // 🔹 New: marks this item as a section header
        public bool IsHeader { get; set; } = false;

        // 🔹 New: header text for display
        public string HeaderText { get; set; }
    }

}
