using System.Text.Json;
using Confluent.Kafka;

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
            Console.WriteLine("DataIngest.dll broker-ip:port,broker-ip:port,... group-id open-meteo-topic-name weatherapi-topic-name");
            return;
        }
        
        var configuration = configurationProvider.GetConfiguration();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => {
            e.Cancel = true;
            cancellationTokenSource.Cancel();
        };
        
        using (IMessageConsumer consumer = new KafkaMessageConsumer(configuration))
        {
            consumer.Subscribe(new []{configuration.OpenMeteoTopicName, configuration.WeatherApiTopicName}, HandleMessage, cancellationTokenSource.Token);
        }
    }

    private static void HandleMessage(Message<string, string> message)
    {
        WeatherMessage weatherMessage = JsonSerializer.Deserialize<WeatherMessage>(message.Value);
        Console.WriteLine(weatherMessage);
    }
}