namespace WeatherApp
{
    public class SensorMeta
    {
        public string? SensorID { get; set; }
        public string? Category { get; set; }
        public string? Symbol { get; set; }
        public string? Unit { get; set; }
        public string? UnitDescription { get; set; }
        public string? Frequency { get; set; }
        public string? SafeLevel { get; set; }
        public string? Reference { get; set; }
        public string? Model { get; set; }

        public string? Location { get; set; }
        public DateTime Installed { get; set; }
        public string? Status { get; set; }
        public DateTime? MaintenanceDate { get; set; }

        public bool IsFlagged { get; set; } = false;
    }
}
