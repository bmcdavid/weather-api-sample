using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Features.AppDetails;

[ApiController]
[Route("api/[controller]")]
public class AppInfoController : ControllerBase
{
    [HttpGet]
    public Task<IReadOnlyCollection<string>> Index()
    {
        var items = new List<string>();
        var buildInfo = Weather.Contracts.BuildInfoAttribute.BuildInfoAttributes;

        foreach (var (key, value) in buildInfo)
        {
            items.Add($"build, {key} = {value}");
        }

        var environmentVariables = Environment.GetEnvironmentVariables()
            .OfType<DictionaryEntry>()
            .OrderBy(o => o.Key);

        foreach (var envItem in environmentVariables)
        {
            var maskedValue = envItem.Value is string { Length: > 0 } s
                ? string.Concat(Enumerable.Range(0, s.Length).Select(s => '*'))
                : "***";
            items.Add($"env, {envItem.Key} = {maskedValue}");
        }

        return Task.FromResult<IReadOnlyCollection<string>>(items);
    }
}
