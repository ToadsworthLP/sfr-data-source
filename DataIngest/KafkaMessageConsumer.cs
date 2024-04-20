using Confluent.Kafka;

namespace DataIngest;

public class KafkaMessageConsumer : IMessageConsumer
{
    private readonly IConsumer<string, string> Consumer;
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

        Consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
    }

    public void Subscribe(IEnumerable<string> topics, Action<ConsumeResult<string, string>> handler, CancellationToken cancellationToken)
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