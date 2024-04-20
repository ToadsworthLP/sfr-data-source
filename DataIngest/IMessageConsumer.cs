using Confluent.Kafka;

namespace DataIngest;

public interface IMessageConsumer : IDisposable
{
    void Subscribe(IEnumerable<string> topics, Action<ConsumeResult<string, string>> handler, CancellationToken cancellationToken);
}