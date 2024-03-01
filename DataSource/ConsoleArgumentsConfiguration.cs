using Confluent.Kafka;

namespace DataSource;

public class ConsoleArgumentsConfiguration : IConfigurationProvider
{
    private Configuration configuration;
    
    public ConsoleArgumentsConfiguration(string[] args)
    {
        if (args == null)
        {
            throw new ArgumentException("Failed to parse program arguments: No arguments provided.");
        }

        if (args.Length == 3)
        {
            configuration = new Configuration
            {
                KafkaAddresses = args[0],
                ForecastTopicName = args[1],
                ActualWeatherTopicName = args[2],
                AckRequirement = Acks.None,
                MaxFlushTimeout = 5
            };
        } else if (args.Length == 5)
        {
            configuration = new Configuration
            {
                KafkaAddresses = args[0],
                ForecastTopicName = args[1],
                ActualWeatherTopicName = args[2],
                AckRequirement = args[3] == "ack-all" ? Acks.All : args[3] == "ack-leader" ? Acks.Leader : Acks.None,
                MaxFlushTimeout = double.Parse(args[4])
            };
        }
        else
        {
            throw new ArgumentException("Failed to parse program arguments: Invalid number of provided arguments.");
        }
    }

    public Configuration GetConfiguration()
    {
        return configuration;
    }
}