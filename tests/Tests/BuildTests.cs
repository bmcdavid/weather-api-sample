using Microsoft.AspNetCore.Mvc.Testing;
using Weather.Contracts;

namespace App.Tests;

public sealed class BuildTests
{
    [Fact]
    public void BuildAttributesShouldBeSet()
    {
        BuildInfoAttribute.BuildInfoAttributes.Should().
            ContainKeys("BuildDateUtc", "BuildNumber", "GitHash", "GitBranch");
    }

    [Fact]
    public void BuildInfoAttributesShouldThrowExceptionWhenKeyOrValueNull()
    {
        var act1 = () => new BuildInfoAttribute(null!, "value");
        var act2 = () => new BuildInfoAttribute("key", null!);

        act1.Should().ThrowExactly<ArgumentNullException>();
        act2.Should().ThrowExactly<ArgumentNullException>();
    }

    [Theory]
    [InlineData("/api/health")]
    [InlineData("/api/weatherforecast")]
    public async Task ProgramShouldBeHealthy(string route)
    {
        using var appFactory = new WebApplicationFactory<Program>();
        using var httpClient = appFactory.CreateClient();

        var result = await httpClient.GetAsync(new Uri(route, UriKind.Relative));

        result.IsSuccessStatusCode.Should().BeTrue();
    }
}
