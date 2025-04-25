using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApp.Models;
[Table("Maintenance")]
public class Maintenance
{
    [Key]
    [Column("maintenance_id")]
    public int MaintenanceId { get; set; }

    [Column("sensor_id")]
    public int SensorId { get; set; }

    [Column("maintainer_id")]
    public int MaintainerId { get; set; }

    [Column("maintenance_date")]
    public string MaintenanceDate { get; set; }

    [Column("maintainer_comments")]
    public string? MaintainerComments { get; set; }
}