namespace DataSource;

public interface IMessagePublisher : IDisposable
{
    Task<bool> Publish(string topic, string key, string value, IDictionary<string, string> headers);
}