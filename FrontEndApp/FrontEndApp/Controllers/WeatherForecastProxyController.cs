using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FrontEndApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastProxyController : ControllerBase
    {
        private readonly ILogger<WeatherForecastProxyController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public WeatherForecastProxyController(
            ILogger<WeatherForecastProxyController> logger,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,"http://localhost:5000/weatherforecast");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<WeatherForecast>>(responseStream);
            }
            else
            {
                return new WeatherForecast[0];
            }
        }
    }
}
