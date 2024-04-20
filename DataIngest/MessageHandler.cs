using System.Text.Json;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;

namespace DataIngest;

public class MessageHandler : IMessageHandler
{
    private WeatherDbContext db;
    private Configuration configuration;
    private CancellationToken cancellationToken;

    public MessageHandler(WeatherDbContext db, Configuration configuration, CancellationToken cancellationToken)
    {
        this.db = db;
        this.configuration = configuration;
        this.cancellationToken = cancellationToken;
    }

    public void HandleMessage(ConsumeResult<string, string> result)
    {
        WeatherMessage weatherMessage = JsonSerializer.Deserialize<WeatherMessage>(result.Message.Value);
        
        WeatherEntry newEntry = new WeatherEntry()
        {
            Timestamp = weatherMessage.timestamp,
            Provider = result.Topic == "openmeteo" ? WeatherDataProvider.OpenMeteo : WeatherDataProvider.WeatherApi,
            Temperature = weatherMessage.temperature,
            TemperatureUnit = weatherMessage.temperature_unit == "f" ? TemperatureUnit.Fahrenheit : TemperatureUnit.Celsius,
            Pressure = weatherMessage.pressure,
            PressureUnit = PressureUnit.Millibar
        };

        newEntry.Timestamp = DateTime.SpecifyKind(newEntry.Timestamp, DateTimeKind.Utc);

        IQueryable<WeatherEntry> duplicatesQueryable = db.WeatherData.Where(e => e.Timestamp == newEntry.Timestamp && e.Provider == newEntry.Provider);
        WeatherEntry[] duplicates = RetryUntilSuccess(duplicatesQueryable.ToArrayAsync(cancellationToken), cancellationToken).Result;
        if (duplicates.Length == 0)
        {
            db.WeatherData.Add(newEntry);
            RetryUntilSuccess(db.SaveChangesAsync(cancellationToken), cancellationToken).Wait();
        }
        else
        {
            Console.WriteLine($"An entry for timestamp {newEntry.Timestamp:s} from provider {newEntry.Provider} already exists. Skipping message.");
        }
    }

    private async Task<T> RetryUntilSuccess<T>(Task<T> task, CancellationToken cancellationToken)
    {
        bool success = false;
        T result = default(T);
        
        while (!success)
        {
            try
            {
                result = await task;
                success = true;
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine($"Failed to access database, retrying in {configuration.RetryInterval} seconds: {e}");
                await Task.Delay(TimeSpan.FromSeconds(configuration.RetryInterval), cancellationToken);
                
                if(cancellationToken.IsCancellationRequested) break;
            }
        }

        return result;
    }
}