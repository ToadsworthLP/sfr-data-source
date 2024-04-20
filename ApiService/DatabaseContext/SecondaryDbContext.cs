using ApiService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiService.DatabaseContext
{
    public class SecondaryDbContext : DbContext
    {
        public SecondaryDbContext(DbContextOptions<SecondaryDbContext> options)
        : base(options)
        { }
        public DbSet<WeatherEntry> WeatherEntries { get; set; }

    }
}
