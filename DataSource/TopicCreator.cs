using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System.Text.RegularExpressions;

namespace DataSource;
internal partial class TopicCreator
{
    [GeneratedRegex(@"Topic '.*' already exists.")]
    private static partial Regex MyRegex();

    public static async Task CreateTopicAsync(string boostrapServers, string topicName)
    {
        using var adminClient = new AdminClientBuilder(new AdminClientConfig
        {
            BootstrapServers = boostrapServers
        }).Build();

        try
        {
            await adminClient.CreateTopicsAsync(
            [
                new() {
                    Name = topicName,
                    NumPartitions = 9,
                    ReplicationFactor = 3,
                    Configs = new Dictionary<string, string>
                    {
                        {  "min.insync.replicas", "2" }
                    }
                }
            ]);
        }
        catch (CreateTopicsException e)
        {
            if (MyRegex().IsMatch(e.Results[0].Error.Reason))
            {
                return;
            }

            Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
        }
    }


}
