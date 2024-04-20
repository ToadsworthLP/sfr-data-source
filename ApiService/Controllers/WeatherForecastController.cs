using ApiService.DatabaseContext;
using ApiService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly PrimaryDbContext _dbContext;
        private readonly SecondaryDbContext _secondaryDbContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, PrimaryDbContext dbContext, SecondaryDbContext secondaryDbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _secondaryDbContext = secondaryDbContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherEntry> Get()
        {
            IEnumerable<WeatherEntry> result = [];
            _logger.LogInformation("GetWeatherForecats requested");
            return _dbContext.WeatherEntries;
        }

        [HttpGet("{provider:int}", Name = "GetWeatherForecastByWeatherDataProvider")]
        public IEnumerable<WeatherEntry> GetByWeatherDataProvider(WeatherDataProvider provider)
        {
            _logger.LogInformation("GetWeatherForecastByWeatherDataProvider requested");
            return _dbContext.WeatherEntries.Where(entry => entry.Provider == provider);
        }
    }
}
