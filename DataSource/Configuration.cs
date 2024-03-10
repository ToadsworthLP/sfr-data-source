using Confluent.Kafka;

namespace DataSource;

public class Configuration
{
    public string KafkaAddresses { get; init; } = "";
    public string ForecastTopicName { get; init; } = "";
    public string ActualWeatherTopicName { get; init; } = "";
    public Acks AckRequirement { get; init; }
    public double RetryInterval { get; init; }
    public double MaxFlushTimeout { get; init; }
}