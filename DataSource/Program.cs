namespace DataSource;

class Program
{
    private static Configuration configuration;
    
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
            Console.WriteLine("DataSource.dll broker-ip:port,broker-ip:port,... ack-all|ack-none|ack-leader max-flush-timeout-seconds retry-interval forecast-topic-name actual-weather-topic-name");
            return;
        }

        configuration = configurationProvider.GetConfiguration();

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
        IDictionary<string, string> headers = new Dictionary<string, string> { { "source", source } };
        string value = await RetryTaskUntilSuccess(() => providerFunc(dateTime), configuration.RetryInterval);
        await RetryTaskUntilSuccess(() => publisher.Publish(topic, dateTime.ToString("s"), value, headers), configuration.RetryInterval);
    }

    private static async Task RetryTaskUntilSuccess(Func<Task> func, double retryInterval)
    {
        bool success = false;
        
        while (!success)
        {
            try
            {
                await func();
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Caught exception in task: {e}\nRetrying in {retryInterval} seconds.");
                await Task.Delay(TimeSpan.FromSeconds(retryInterval));
            }
        }
    }
    
    private static async Task<T> RetryTaskUntilSuccess<T>(Func<Task<T>> func, double retryInterval)
    {
        bool success = false;
        T result = default(T);
        
        while (!success)
        {
            try
            {
                result = await func();
                success = true;
            }
            catch (Exception e)
            {
                await Task.Delay(TimeSpan.FromSeconds(retryInterval));
            }
        }

        return result;
    }
}