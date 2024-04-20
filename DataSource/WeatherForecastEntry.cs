using System.Globalization;

namespace DataSource;

public class WeatherForecastEntry
{
    public DateTime Timestamp { get; set; }
    public double Temperature { get; set; }
    public string TemperatureUnit { get; set; }
    public double Pressure { get; set; }
    public string PressureUnit { get; set; }

    private static NumberFormatInfo numberFormatInfo;

    static WeatherForecastEntry()
    {
        numberFormatInfo = new NumberFormatInfo
        {
            NumberDecimalSeparator = ".",
            NumberGroupSeparator = ""
        };
    }

    public WeatherForecastEntry(DateTime timestamp, double temperature, string temperatureUnit, double pressure, string pressureUnit)
    {
        Timestamp = timestamp;
        Temperature = temperature;
        TemperatureUnit = temperatureUnit;
        Pressure = pressure;
        PressureUnit = pressureUnit;
    }

    public override string ToString()
    {
        return $"{{ \"timestamp\": \"{Timestamp:s}\", \"temperature\": {Temperature.ToString(numberFormatInfo)}, \"temperature_unit\": \"{TemperatureUnit}\", \"pressure\": {Pressure.ToString(numberFormatInfo)}, \"pressure_unit\": \"{PressureUnit}\" }}";
    }
}