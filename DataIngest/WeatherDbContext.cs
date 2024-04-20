using Microsoft.EntityFrameworkCore;

namespace DataIngest;

public class WeatherDbContext : DbContext
{
    private string connectionString;
    
    public DbSet<WeatherEntry> WeatherData { get; set; }
    
    public WeatherDbContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherEntry>()
            .ToTable("weather_data")
            .Property(e => e.Provider).HasConversion<int>();
        
        modelBuilder.Entity<WeatherEntry>()
            .Property(e => e.TemperatureUnit).HasConversion<int>();
        
        modelBuilder.Entity<WeatherEntry>()
            .Property(e => e.PressureUnit).HasConversion<int>();
        
        base.OnModelCreating(modelBuilder);
    }
}