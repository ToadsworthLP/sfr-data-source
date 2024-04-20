
using ApiService.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ApiService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // For EntityFramework
            builder.Services.AddDbContext<PrimaryDbContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // For EntityFramework
            builder.Services.AddDbContext<SecondaryDbContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("BackupConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
