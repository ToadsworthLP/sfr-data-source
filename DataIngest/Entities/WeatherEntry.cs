using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataIngest.Entities;

public class WeatherEntry
{
    [Key, Column("id")] public Guid Id { get; set; }
    [Column("timestamp")] public DateTime Timestamp { get; set; }
    [Column("provider")] public WeatherDataProvider Provider { get; set; }
    [Column("temperature")] public double Temperature { get; set; }
    [Column("temperature_unit")] public TemperatureUnit TemperatureUnit { get; set; }
    [Column("pressure")] public double Pressure { get; set; }
    [Column("pressure_unit")] public PressureUnit PressureUnit { get; set; }
}