using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApp.Core.Models;

[Table("Sites")]
public class Site
{
    [Key]
    [Column("site_id")]
    public int SiteId { get; set; }

    [Column("type")]
    public string Type { get; set; }

    [Column("longitude")]
    public double Longitude { get; set; }

    [Column("latitude")]
    public double Latitude { get; set; }
}
