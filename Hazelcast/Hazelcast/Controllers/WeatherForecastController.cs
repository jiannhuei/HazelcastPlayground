using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hazelcast.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly HazelcastOptions _options;


        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, HazelcastOptions options)
        {
            _logger = logger;
            _options = options;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {

            await using var client = await HazelcastClientFactory.StartNewClientAsync(_options);

            //await using var client = await HazelcastClientFactory.StartNewClientAsync(o =>
            //{                 
            //    o.Networking.Addresses.Add("127.0.0.1:5701");
            //    o.Authentication.ConfigureUsernamePasswordCredentials("admin", "Pbb1223@");
            //    o.ClusterName = "pbbtrain";                
            //    //o.ClientName = "MC-Client-pbbtrain";
            //});

            await using var set = await client.GetSetAsync<string>("my-distributed-set");           
            await set.AddAsync("item1");
            await set.AddAsync("item1");

            await foreach (var item in set)
            {
                Console.WriteLine(item);
            }

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}