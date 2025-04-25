using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApp.Models;

[Table("Measurements")]
public class Measurement
{
    [Key]
    [Column("measurement_id")]
    public int MeasurementId { get; set; }

    [Column("quantity_id")]
    public int QuantityId { get; set; }

    [Column("date")]
    public string Date { get; set; }

    [Column("time")]
    public string Time { get; set; }

    [Column("value")]
    public double? Value { get; set; }
}
