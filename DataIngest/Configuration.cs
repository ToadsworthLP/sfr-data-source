namespace DataIngest;

public class Configuration
{
    public string KafkaAddresses { get; init; }
    public string OpenMeteoTopicName { get; init; }
    public string WeatherApiTopicName { get; init; }
    public string GroupId;
    public string DbConnectionString { get; init; }
    public double RetryInterval { get; init; }
}