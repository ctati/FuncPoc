using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using web.Models;
using Dapr.Client;
using web.Proxy;

namespace web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;
 
#if DAPR_DIRECT
    private readonly DaprClient _daprClient;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration, DaprClient daprClient)
    {
        _logger = logger;
        _configuration = configuration;
        _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));

        var baseUrl = _configuration.GetValue<string>("API_BASE_URL");
        _logger.Log(LogLevel.Information, "API URL: {baseUrl}", baseUrl);
    }
#else
    private readonly IWeatherForecastClient _weatherClient;

   public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IWeatherForecastClient weatherClient)
    {
        _logger = logger;
        _configuration = configuration;
        _weatherClient = weatherClient ?? throw new ArgumentNullException(nameof(weatherClient));
    }
#endif

    public async Task<IActionResult> Index()
    {
#if DAPR_DIRECT
        var forecasts = await _daprClient.InvokeMethodAsync<IEnumerable<WeatherForecast>>(HttpMethod.Get, "poc-api", "weatherforecast");
#else
        var forecasts = await _weatherClient.GetWeatherForecastAsync();
#endif
        ViewData["WeatherForecastData"] = forecasts;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
