using Microsoft.AspNetCore.Mvc;

namespace Resturants.API.Controllers;

public class TemperatureRequest { 
    public int Min { get; set; }
    public int Max { get; set; }

}


[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService)
    {
        _logger = logger;
        _weatherForecastService = weatherForecastService;
    }

    //[HttpGet]
    //[Route("example")]
    //public IEnumerable<WeatherForecast> Get()
    //{
    //    var result = _weatherForecastService.Get();
    //    return result;
    //}

    //[HttpGet("{take}/currentDay")]
    //public ObjectResult Get([FromQuery]int max, [FromRoute] int take)
    
    //public IActionResult Get([FromQuery]int max, [FromRoute] int take)
    //{
    //    var result = _weatherForecastService.Get().First();
    //    //Response.StatusCode = 400;
    //    //return result;
    //    //return StatusCode(200, result);
    //    return BadRequest(result);
    //}

    [HttpPost]
    public string Hello([FromBody] string name) {
        return $"Hello {name}";
    }


    [HttpPost("generate")]
    public IActionResult Generate([FromQuery] int count, [FromBody] TemperatureRequest request) {
        if (count > 0 && (request.Max > request.Min ))
        {
            var result = _weatherForecastService.Get(count, request.Min, request.Max);
            return Ok(result);
        }
        return BadRequest("Either number of results to be generated is less than zero or max temperature is less than minimum temperature");
        }

}
