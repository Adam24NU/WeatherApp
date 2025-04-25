using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApp.Models
{
    [Table("PhysicalQuantities")]
    public class PhysicalQuantity
    {
        [Key]
        [Column("quantity_id")]
        public int QuantityId { get; set; }

        [Column("sensor_id")]
        public int SensorId { get; set; }

        [Column("quantity_name")]
        [MaxLength(50)]
        public string QuantityName { get; set; }

        [Column("symbol")]
        [MaxLength(10)]
        public string Symbol { get; set; }

        [Column("unit")]
        [MaxLength(10)]
        public string Unit { get; set; }

        [Column("unit_description")]
        [MaxLength(50)]
        public string UnitDescription { get; set; }

        [Column("measurement_frequency")]
        [MaxLength(10)]
        public string MeasurementFrequency { get; set; }

        [Column("safe_level")]
        [MaxLength(10)]
        public string? SafeLevel { get; set; }

        [Column("reference")]
        [MaxLength(255)]
        public string? Reference { get; set; }

        [Column("sensor_url")]
        [MaxLength(255)]
        public string SensorUrl { get; set; }

    }
}
