using System.Globalization;

namespace DataSource;

public class WeatherForecastEntry
{
    public DateTime Timestamp { get; init; }
    public double Temperature { get; init; }
    public string TemperatureUnit { get; init; }
    public double Pressure { get; init; }
    public string PressureUnit { get; init; }

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