namespace DataIngest;

class Program
{
    static void Main(string[] args)
    {
        IConfigurationProvider configurationProvider;
        try
        {
            configurationProvider = new ConsoleArgumentsConfiguration(args);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("DataIngest.dll broker-ip:port,broker-ip:port,... group-id open-meteo-topic-name weatherapi-topic-name db-connection-string retry-interval");
            return;
        }
        
        var configuration = configurationProvider.GetConfiguration();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => {
            e.Cancel = true;
            cancellationTokenSource.Cancel();
        };

        using WeatherDbContext db = new WeatherDbContext(configuration.DbConnectionString);
        using IMessageConsumer consumer = new KafkaMessageConsumer(configuration);
        
        IMessageHandler handler = new MessageHandler(db, configuration, cancellationTokenSource.Token);
        consumer.Subscribe(new []{configuration.OpenMeteoTopicName, configuration.WeatherApiTopicName}, handler.HandleMessage, cancellationTokenSource.Token);
    }
}