using App.Api.Features.Forecast;
using Microsoft.Extensions.Logging;

namespace App.Tests;

public class WeatherForecastControllerTests
{
    private Mock<ILogger<WeatherForecastController>> logger = new();

    [Fact]
    public void ShouldGetForecasts()
    {
        var controller = new WeatherForecastController(logger.Object);

        var result = controller.Get();

        result.Count().Should().Be(5);
    }
}
