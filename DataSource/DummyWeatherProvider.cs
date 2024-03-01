namespace DataSource;

public class DummyWeatherProvider : IWeatherProvider
{
    public async Task<string> GetActualWeather(DateTime dateTime)
    {
        return GetDummyData();
    }

    public async Task<string> GetForecast(DateTime dateTime)
    {
        return GetDummyData();
    }
    
    private string GetDummyData()
    {
        return "{temperature: 15.0, rain: true}";
    }
}