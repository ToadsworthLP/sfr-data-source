using ApiService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly AppDbContext _dbContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherEntry> Get()
        {
            _logger.LogInformation("GetWeatherForecats requested");
            return _dbContext.WeatherEntries;
        }
    }
}
