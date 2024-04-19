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
            Console.WriteLine("DataSource.dll broker-ip:port,broker-ip:port,... openmeteo-topic-name weatherapi-topic-name");
            Console.WriteLine("DataSource.dll broker-ip:port,broker-ip:port,... ack-all|ack-none|ack-leader max-flush-timeout-seconds retry-interval-seconds open-meteo-topic-name weatherapi-topic-name");
            return;
        }

        var configuration = configurationProvider.GetConfiguration();

        Task.WaitAll(
            TopicCreator.CreateTopicAsync(configuration.KafkaAddresses, configuration.OpenMeteoTopicName),
            TopicCreator.CreateTopicAsync(configuration.KafkaAddresses, configuration.WeatherApiTopicName)
        );
        
        IMessagePublisher publisher = new KafkaMessagePublisher(configuration);
        using (publisher)
        {
            IWeatherProvider openMeteoWeatherProvider = new OpenMeteoWeatherProvider();
            IWeatherProvider weatherApiWeatherProvider = new WeatherApiWeatherProvider();
            
            Task.WaitAll(
                SendWeatherMessage(() => openMeteoWeatherProvider.GetForecast(), publisher, configuration.OpenMeteoTopicName, openMeteoWeatherProvider.ProviderName, configuration),
                SendWeatherMessage(() => weatherApiWeatherProvider.GetForecast(), publisher, configuration.WeatherApiTopicName, weatherApiWeatherProvider.ProviderName, configuration)
            );
        }
    }

    private static async Task SendWeatherMessage(Func<Task<IEnumerable<WeatherForecastEntry>>> providerFunc, IMessagePublisher publisher, string topic, string source, Configuration configuration)
    {
        IDictionary<string, string> headers = new Dictionary<string, string> { { "source", source } };
        IEnumerable<WeatherForecastEntry> value = await RetryTaskUntilSuccess(providerFunc, configuration.RetryInterval);

        foreach (WeatherForecastEntry entry in value)
        {
            string key = entry.Timestamp.ToString("s");
            await RetryTaskUntilSuccess(() => publisher.Publish(topic, key, entry.ToString(), headers), configuration.RetryInterval);
            Console.WriteLine($"Successfully sent message with key {key} to topic {topic}.");
        }
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