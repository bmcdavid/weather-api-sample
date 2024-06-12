using App.Api.Features.AppDetails;

namespace App.Tests;

public class AppInfoControllerTests
{
    [Fact]
    public async Task ShouldGetAppInfo()
    {
        var controller = new AppInfoController();

        var result = await controller.Get();

        result.Count.Should().BeGreaterThanOrEqualTo(5);
    }
}
