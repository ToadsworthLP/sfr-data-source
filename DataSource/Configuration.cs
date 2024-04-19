using Confluent.Kafka;

namespace DataSource;

public class Configuration
{
    public string KafkaAddresses { get; init; } = "";
    public string OpenMeteoTopicName { get; init; } = "";
    public string WeatherApiTopicName { get; init; } = "";
    public Acks AckRequirement { get; init; }
    public double RetryInterval { get; init; }
    public double MaxFlushTimeout { get; init; }
}