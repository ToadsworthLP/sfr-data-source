using System.Globalization;
using System.Text.Json;

namespace DataSource;

public class WeatherApiWeatherProvider : IWeatherProvider
{
    public string ProviderName => "WeatherAPI";
    
    private HttpClient httpClient = new();

    public async Task<IEnumerable<WeatherForecastEntry>> GetForecast()
    {
        using HttpResponseMessage response = await httpClient.GetAsync(
            "https://api.weatherapi.com/v1/forecast.json?key=dd267ead7e534694abf200151241103&q=48.2085,16.3721&days=1&aqi=no&alerts=no");
        response.EnsureSuccessStatusCode();
        string responseStr = await response.Content.ReadAsStringAsync();
        WeatherApiResponse responseBody = JsonSerializer.Deserialize<WeatherApiResponse>(responseStr);

        List<WeatherForecastEntry> entries = new List<WeatherForecastEntry>();
        foreach (WeatherApiResponse.Hour hour in responseBody.forecast.forecastday[0].hour)
        {
            entries.Add(new WeatherForecastEntry(
                DateTime.Parse(hour.time, CultureInfo.InvariantCulture),
                hour.temp_f,
                "f",
                hour.pressure_mb,
                "mb"
            ));
        }
            
        return entries;
    }
    
    private class WeatherApiResponse
    {
        public Forecast forecast { get; set; }
        
        public class Forecast
        {
            public Day[] forecastday { get; set; }
        }

        public class Day
        {
            public Hour[] hour { get; set; }
        }
        
        public class Hour
        {
            public string time { get; set; }
            public double temp_f { get; set; }
            public double pressure_mb { get; set; }
        }
    }
}