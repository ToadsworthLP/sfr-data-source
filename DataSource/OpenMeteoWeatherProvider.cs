using System.Globalization;
using System.Text.Json;

namespace DataSource;

public class OpenMeteoWeatherProvider : IWeatherProvider
{
    public string ProviderName => "OpenMeteo";

    private HttpClient httpClient = new();

    public async Task<IEnumerable<WeatherForecastEntry>> GetForecast()
    {
        using HttpResponseMessage response = await httpClient.GetAsync(
            "https://api.open-meteo.com/v1/forecast?latitude=48.2085&longitude=16.3721&hourly=temperature_2m,pressure_msl&timezone=Europe%2FVienna&forecast_days=1");
        response.EnsureSuccessStatusCode();
        string responseStr = await response.Content.ReadAsStringAsync();
        OpenMeteoResponse responseBody = JsonSerializer.Deserialize<OpenMeteoResponse>(responseStr);

        List<WeatherForecastEntry> entries = new List<WeatherForecastEntry>();
        for (int i = 0; i < responseBody.hourly.time.Length; i++)
        {
            string timestamp = responseBody.hourly.time[i];
            double temperature = responseBody.hourly.temperature_2m[i];
            double pressure = responseBody.hourly.pressure_msl[i];
            
            entries.Add(new WeatherForecastEntry(
                DateTime.Parse(timestamp, CultureInfo.InvariantCulture),
                temperature,
                "c",
                pressure,
                "mb"
            ));
        }

        return entries;
    }

    private class OpenMeteoResponse
    {
        public Hourly hourly { get; set; }
        
        public class Hourly
        {
            public string[] time { get; set; }
            public double[] temperature_2m { get; set; }
            public double[] pressure_msl { get; set; }
        }
    }
}