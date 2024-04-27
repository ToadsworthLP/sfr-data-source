using Confluent.Kafka;

namespace DataSource;

public class ConsoleArgumentsConfiguration : IConfigurationProvider
{
    private readonly Configuration Configuration;
    
    public ConsoleArgumentsConfiguration(string[] args)
    {
        if (args == null)
        {
            throw new ArgumentException("Failed to parse program arguments: No arguments provided.");
        }

        if (args.Length == 7)
        {
            try
            {
                Configuration = new Configuration
                {
                    KafkaAddresses = args[0],
                    SchemaRegistryAddresses = args[1],
                    AckRequirement =
                        args[2] == "ack-all" ? Acks.All : args[2] == "ack-leader" ? Acks.Leader : Acks.None,
                    MaxFlushTimeout = double.Parse(args[3]),
                    RetryInterval = double.Parse(args[4]),
                    OpenMeteoTopicName = args[5],
                    WeatherApiTopicName = args[6]
                };
                return;
            }
            catch (FormatException e)
            {
                throw new ArgumentException($"Failed to parse program arguments: {e.Message}.");
            }
        }

        throw new ArgumentException("Failed to parse program arguments: Invalid number of provided arguments.");
    }

    public Configuration GetConfiguration()
    {
        return Configuration;
    }
}