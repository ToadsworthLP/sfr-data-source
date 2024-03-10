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

        if (args.Length == 3)
        {
            Configuration = new Configuration
            {
                KafkaAddresses = args[0],
                AckRequirement = Acks.None,
                MaxFlushTimeout = 10.0,
                RetryInterval = 10.0,
                ForecastTopicName = args[1],
                ActualWeatherTopicName = args[2]
            };
            return;
        } 
        
        if (args.Length == 6)
        {
            try
            {
                Configuration = new Configuration
                {
                    KafkaAddresses = args[0],
                    AckRequirement =
                        args[1] == "ack-all" ? Acks.All : args[1] == "ack-leader" ? Acks.Leader : Acks.None,
                    MaxFlushTimeout = double.Parse(args[2]),
                    RetryInterval = double.Parse(args[3]),
                    ForecastTopicName = args[4],
                    ActualWeatherTopicName = args[5]
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