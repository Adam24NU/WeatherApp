using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ConfigSettings")]
public class ConfigSetting
{
    [Key]
    [Column("setting_id")]
    public int SettingId { get; set; }

    [Column("sensor_id")]
    public int SensorId { get; set; }

    [Column("setting_name")]
    public string SettingName { get; set; }

    [Column("min_value")]
    public double? MinValue { get; set; }

    [Column("max_value")]
    public double? MaxValue { get; set; }

    [Column("current_value")]
    public double? CurrentValue { get; set; }
}
