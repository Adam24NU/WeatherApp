using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApp.Models;

[Table("IncidentMeasurements")]
public class IncidentMeasurement
{
    [Key]
    [Column("incident_id")]
    public int IncidentId { get; set; }

    [Key]
    [Column("measurement_id")]
    public int MeasurementId { get; set; }
}

