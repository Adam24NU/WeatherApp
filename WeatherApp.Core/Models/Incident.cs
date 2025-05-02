using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApp.Core.Models;

[Table("Incidents")]
public class Incident
{
    [Key]
    [Column("incident_id")]
    public int IncidentId { get; set; }

    [Column("responder_id")]
    public int ResponderId { get; set; }

    [Column("priority")]
    public string? Priority { get; set; }

    [Column("responder_comments")]
    public string? ResponderComments { get; set; }

    [Column("resolved_date")]
    public string? ResolvedDate { get; set; }
}