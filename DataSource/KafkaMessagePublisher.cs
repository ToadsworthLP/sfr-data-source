using System.Text;
using Confluent.Kafka;

namespace DataSource;

public class KafkaMessagePublisher : IMessagePublisher
{
    private readonly IProducer<string, string> Producer;
    private readonly Configuration Configuration;

    public KafkaMessagePublisher(Configuration configuration)
    {
        this.Configuration = configuration;

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration.KafkaAddresses,
            Acks = configuration.AckRequirement,
            Partitioner = Partitioner.ConsistentRandom
        };
        
        Producer = new ProducerBuilder<string, string>(producerConfig).Build();
    }

    public async Task Publish(string topic, string key, string value, IDictionary<string, string> headers)
    {
        var msgHeaders = new Headers();
        foreach (var header in headers)
        {
            msgHeaders.Add(header.Key, Encoding.UTF8.GetBytes(header.Value));
        }
            
        var deliveryResult = await Producer.ProduceAsync(topic, new Message<string, string>
        {
            Key = key,
            Headers = msgHeaders,
            Value = value,
        });
    }

    public void Dispose()
    {
        Console.WriteLine($"Flushing message producer (max. {Configuration.MaxFlushTimeout} seconds).");
        Producer.Flush(TimeSpan.FromSeconds(Configuration.MaxFlushTimeout));
        Producer.Dispose();
    }
}