using System.Text;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace DataSource;

public class KafkaMessagePublisher : IMessagePublisher
{
    private readonly IProducer<string, WeatherMessage> Producer;
    private readonly ISchemaRegistryClient SchemaRegistryClient;
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
        
        var schemaRegistryConfig = new SchemaRegistryConfig
        {
            Url = configuration.SchemaRegistryAddresses
        };
        
        var avroSerializerConfig = new AvroSerializerConfig
        {
            BufferBytes = 100
        };
        
        SchemaRegistryClient = new CachedSchemaRegistryClient(schemaRegistryConfig);

        Producer = new ProducerBuilder<string, WeatherMessage>(producerConfig)
            .SetValueSerializer(new AvroSerializer<WeatherMessage>(SchemaRegistryClient, avroSerializerConfig))
            .Build();
    }
    
    public void Dispose()
    {
        Console.WriteLine($"Flushing message producer (max. {Configuration.MaxFlushTimeout} seconds).");
        Producer.Flush(TimeSpan.FromSeconds(Configuration.MaxFlushTimeout));
        Producer.Dispose();
    }

    public async Task Publish(string topic, string key, WeatherMessage message, IDictionary<string, string> headers)
    {
        var msgHeaders = new Headers();
        foreach (var header in headers)
        {
            msgHeaders.Add(header.Key, Encoding.UTF8.GetBytes(header.Value));
        }
            
        var deliveryResult = await Producer.ProduceAsync(topic, new Message<string, WeatherMessage>
        {
            Key = key,
            Headers = msgHeaders,
            Value = message,
        });
    }
}