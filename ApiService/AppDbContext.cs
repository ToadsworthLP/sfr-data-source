using ApiService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiService
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        { }
        public DbSet<WeatherEntry> WeatherEntries { get; set; }

    }
}
