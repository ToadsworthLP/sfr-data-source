namespace DataSource;

public interface IWeatherProvider
{
    public string ProviderName { get; }
    public Task<IEnumerable<WeatherForecastEntry>> GetForecast();
}