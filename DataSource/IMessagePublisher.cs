namespace DataSource;

public interface IMessagePublisher : IDisposable
{
    Task Publish(string topic, string key, string value, IDictionary<string, string> headers);
}