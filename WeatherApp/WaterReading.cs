namespace WeatherApp.Models
{
    public class WaterReading
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Nitrate { get; set; }
        public string Nitrite { get; set; }
        public string Phosphate { get; set; }
        public string EC { get; set; }
        
        // New: computed property
        public bool IsThresholdBreached
        {
            get
            {
                double.TryParse(Nitrate, out var nitrateVal);
                double.TryParse(Nitrite, out var nitriteVal);
                double.TryParse(Phosphate, out var phosphateVal);
                double.TryParse(EC, out var ecVal);

                return nitrateVal > 25 || nitriteVal > 1.50 || phosphateVal > 0.05;
            }
        }
    }
}
