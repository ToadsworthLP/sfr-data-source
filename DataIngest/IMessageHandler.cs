using Confluent.Kafka;

namespace DataIngest;

public interface IMessageHandler
{
    void HandleMessage(ConsumeResult<string, string> result);
}