using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApp.Models;

[Table("Sensors")]
public class Sensor
{
    [Key]
    [Column("sensor_id")]
    public int SensorId { get; set; }

    [Column("site_id")]
    public int SiteId { get; set; }

    [Column("sensor_type")]
    public string SensorType { get; set; }

    [Column("name")]
    public string Name { get; set; }
}