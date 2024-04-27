using Confluent.Kafka;
using DataIngest.Entities;

namespace DataIngest;

public interface IMessageConsumer : IDisposable
{
    void Subscribe(IEnumerable<string> topics, Action<ConsumeResult<string, WeatherMessage>> handler, CancellationToken cancellationToken);
}