using Confluent.Kafka;
using DataIngest.Entities;

namespace DataIngest;

public interface IMessageHandler
{
    void HandleMessage(ConsumeResult<string, WeatherMessage> result);
}