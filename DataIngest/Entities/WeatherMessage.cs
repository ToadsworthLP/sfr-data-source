using System.Globalization;

namespace DataIngest.Entities;

public class WeatherMessage
{
    public DateTime timestamp { get; set; }
    public string provider { get; set; }
    public double temperature { get; set; }
    public string temperature_unit { get; set; }
    public double pressure { get; set; }
    public string pressure_unit { get; set; }

    private static NumberFormatInfo numberFormatInfo;

    static WeatherMessage()
    {
        numberFormatInfo = new NumberFormatInfo
        {
            NumberDecimalSeparator = ".",
            NumberGroupSeparator = ""
        };
    }

    public override string ToString()
    {
        return $"{{ \"timestamp\": \"{timestamp:s}\", \"temperature\": {temperature.ToString(numberFormatInfo)}, \"temperature_unit\": \"{temperature_unit}\", \"pressure\": {pressure.ToString(numberFormatInfo)}, \"pressure_unit\": \"{pressure_unit}\" }}";
    }
}