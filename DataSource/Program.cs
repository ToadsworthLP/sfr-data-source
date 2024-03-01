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
            Console.WriteLine("DataSource.dll broker-ip:port,broker-ip:port,... forecast-topic-name actual-weather-topic-name");
            Console.WriteLine("DataSource.dll broker-ip:port,broker-ip:port,... ack-all|ack-none|ack-leader max-flush-timeout-seconds forecast-topic-name actual-weather-topic-name");
            return;
        }

        Configuration configuration = configurationProvider.GetConfiguration();

        IWeatherProvider weatherProvider = new DummyWeatherProvider();
        IMessagePublisher publisher = new KafkaMessagePublisher(configuration);
        using (publisher)
        {
            IList<Task> tasks = new List<Task>();
            
            tasks.Add(SendWeatherMessage(dateTime => weatherProvider.GetActualWeather(dateTime), publisher, DateTime.Now, configuration.ActualWeatherTopicName, "dummy-provider"));
            tasks.Add(SendWeatherMessage(dateTime => weatherProvider.GetForecast(dateTime), publisher, DateTime.Now + TimeSpan.FromDays(1), configuration.ForecastTopicName, "dummy-provider"));

            Task.WhenAll(tasks).Wait();
        }
    }

    private static async Task SendWeatherMessage(Func<DateTime, Task<string>> providerFunc, IMessagePublisher publisher, DateTime dateTime, string topic, string source)
    {
        IDictionary<string, string> headers = new Dictionary<string, string> {{ "source", source }};
        string value = await providerFunc.Invoke(dateTime);
        await publisher.Publish(topic, dateTime.ToString("s"), value, headers);
    }
}