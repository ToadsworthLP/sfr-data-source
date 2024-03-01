namespace DataSource;

public interface IWeatherProvider
{
    public Task<string> GetActualWeather(DateTime dateTime);
    public Task<string> GetForecast(DateTime dateTime);
}