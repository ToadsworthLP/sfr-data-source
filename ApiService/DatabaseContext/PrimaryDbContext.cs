using ApiService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiService.DatabaseContext
{
    public class PrimaryDbContext : DbContext
    {
        public PrimaryDbContext(DbContextOptions<PrimaryDbContext> options)
        : base(options)
        { }
        public DbSet<WeatherEntry> WeatherEntries { get; set; }

    }
}
