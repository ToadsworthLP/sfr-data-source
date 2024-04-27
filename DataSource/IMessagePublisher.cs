namespace DataSource;

public interface IMessagePublisher : IDisposable
{
    Task Publish(string topic, string key, WeatherMessage message, IDictionary<string, string> headers);
}