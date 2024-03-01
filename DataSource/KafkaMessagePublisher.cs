using System.Text;
using Confluent.Kafka;

namespace DataSource;

public class KafkaMessagePublisher : IDisposable, IMessagePublisher
{
    private IProducer<string, string> producer;
    private readonly Configuration configuration;

    public KafkaMessagePublisher(Configuration configuration)
    {
        this.configuration = configuration;

        var producerConfig = new ProducerConfig()
        {
            BootstrapServers = configuration.KafkaAddresses,
            Acks = configuration.AckRequirement
        };
        
        producer = new ProducerBuilder<string, string>(producerConfig).Build();
    }

    public async Task<bool> Publish(string topic, string key, string value, IDictionary<string, string> headers)
    {
        try
        {
            Headers msgHeaders = new Headers();
            foreach (var header in headers)
            {
                msgHeaders.Add(header.Key, Encoding.UTF8.GetBytes(header.Value));
            }
            
            var deliveryResult = await producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = key,
                Headers = msgHeaders,
                Value = value,
            });
            
            return true;
        }
        catch (ProduceException<Null, string> e)
        {
            return false;
        }
    }

    public void Dispose()
    {
        Console.WriteLine($"Flushing message producer (max. {configuration.MaxFlushTimeout} seconds).");
        producer.Flush(TimeSpan.FromSeconds(configuration.MaxFlushTimeout));
        producer.Dispose();
    }
}