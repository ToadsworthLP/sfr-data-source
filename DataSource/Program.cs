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
        catch (ArgumentException)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("DataSource.dll broker-ip:port,broker-ip:port,... forecast-topic-name actual-weather-topic-name");
            Console.WriteLine("DataSource.dll broker-ip:port,broker-ip:port,... ack-all|ack-none|ack-leader max-flush-timeout-seconds retry-interval forecast-topic-name actual-weather-topic-name");
            return;
        }

        var config = configurationProvider.GetConfiguration();

        Task.WaitAll(
            TopicCreator.CreateTopicAsync(config.KafkaAddresses, config.ActualWeatherTopicName),
            TopicCreator.CreateTopicAsync(config.KafkaAddresses, config.ActualWeatherTopicName + "-fareinheit"),
            TopicCreator.CreateTopicAsync(config.KafkaAddresses, config.ForecastTopicName),
            TopicCreator.CreateTopicAsync(config.KafkaAddresses, config.ForecastTopicName + "-fareinheit"));

        IWeatherProvider weatherProvider = new DummyWeatherProvider();
        using IMessagePublisher publisher = new KafkaMessagePublisher(config);
        Task.WaitAll(
            SendWeatherMessage(weatherProvider.GetActualWeather, publisher, DateTime.Now, config.ActualWeatherTopicName, config.RetryInterval, "dummy-provider"),
            SendWeatherMessage(weatherProvider.GetForecast, publisher, DateTime.Now + TimeSpan.FromDays(1), config.ForecastTopicName, config.RetryInterval, "dummy-provider"));
    }

    private static async Task SendWeatherMessage(Func<DateTime, Task<string>> providerFunc, IMessagePublisher publisher, DateTime dateTime, string topic, double retryInterval, string source)
    {
        var headers = new Dictionary<string, string> { { "source", source } };
        string value = await RetryTaskUntilSuccess(() => providerFunc(dateTime), retryInterval);
        await RetryTaskUntilSuccess(() => publisher.Publish(topic, dateTime.ToString("s"), value, headers), retryInterval);
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
        T? result = default;
        
        while (!success)
        {
            try
            {
                result = await func();
                success = true;
            }
            catch (Exception)
            {
                await Task.Delay(TimeSpan.FromSeconds(retryInterval));
            }
        }

        return result!;
    }
}