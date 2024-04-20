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

        if (args.Length == 6)
        {
            try
            {
                Configuration = new Configuration
                {
                    KafkaAddresses = args[0],
                    GroupId = args[1],
                    OpenMeteoTopicName = args[2],
                    WeatherApiTopicName = args[3],
                    DbConnectionString = args[4],
                    RetryInterval = double.Parse(args[5])
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