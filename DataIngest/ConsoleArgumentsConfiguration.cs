using Confluent.Kafka;

namespace DataIngest;

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
                    GroupId = args[2],
                    OpenMeteoTopicName = args[3],
                    WeatherApiTopicName = args[4],
                    DbConnectionString = args[5],
                    RetryInterval = double.Parse(args[6])
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