using Confluent.Kafka;

namespace DataIngest;

public interface IMessageConsumer : IDisposable
{
    void Subscribe(IEnumerable<string> topics, Action<Message<string, string>> handler, CancellationToken cancellationToken);
}