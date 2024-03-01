namespace DataSource;

class Program
{
    static void Main(string[] args)
    {
        IConfigurationProvider configurationProvider;
        try
        {
            configurationProvider = new ConsoleArgumentsConfiguration(args);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("DataSource.dll \"kafka-ip:kafka-port\" \"forecast-topic-name\" \"actual-weather-topic-name\"");
            Console.WriteLine("DataSource.dll \"kafka-ip:kafka-port\" \"forecast-topic-name\" \"actual-weather-topic-name\" \"ack-all\"|\"ack-none\"|\"ack-leader\" \"max-flush-timeout-seconds\"");
            return;
        }

        Configuration configuration = configurationProvider.GetConfiguration();

        IMessagePublisher publisher = new KafkaMessagePublisher(configuration);
        using (publisher)
        {
            IDictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "source", "weather.com" }
            };
            
            publisher.Publish(configuration.ForecastTopicName, DateTime.Now.ToString("s"), "{}", headers).Wait();
        }
    }
}