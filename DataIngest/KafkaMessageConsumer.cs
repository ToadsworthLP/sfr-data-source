using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using DataIngest.Entities;

namespace DataIngest;

public class KafkaMessageConsumer : IMessageConsumer
{
    private readonly IConsumer<string, WeatherMessage> Consumer;
    private readonly ISchemaRegistryClient SchemaRegistryClient;
    private readonly Configuration Configuration;

    public KafkaMessageConsumer(Configuration configuration)
    {
        Configuration = configuration;

        var consumerConfig = new ConsumerConfig()
        {
            BootstrapServers = configuration.KafkaAddresses,
            GroupId = configuration.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };
        
        var schemaRegistryConfig = new SchemaRegistryConfig
        {
            Url = configuration.SchemaRegistryAddresses
        };
        
        SchemaRegistryClient = new CachedSchemaRegistryClient(schemaRegistryConfig);

        Consumer = new ConsumerBuilder<string, WeatherMessage>(consumerConfig)
            .SetValueDeserializer(new AvroDeserializer<WeatherMessage>(SchemaRegistryClient).AsSyncOverAsync())
            .Build();
    }

    public void Subscribe(IEnumerable<string> topics, Action<ConsumeResult<string, WeatherMessage>> handler, CancellationToken cancellationToken)
    {
        Consumer.Subscribe(topics);
        
        try {
            while (true) {
                var result = Consumer.Consume(cancellationToken);
                Console.WriteLine($"Received message with key {result.Message.Key} from topic {result.Topic}.");

                try
                {
                    handler(result);
                    Consumer.Commit(result);
                    Console.WriteLine($"Successfully handled message with key {result.Message.Key} from topic {result.Topic}.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occured when handling message with key {result.Message.Key} in topic {result.Topic}: {e}");
                }
            }
        }
        catch (OperationCanceledException) {
            Console.WriteLine("Shutting down...");
        }
    }

    public void Dispose()
    {
        Consumer.Dispose();
    }
}