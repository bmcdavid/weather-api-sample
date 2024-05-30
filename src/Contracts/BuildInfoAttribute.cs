namespace Weather.Contracts;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class BuildInfoAttribute(string key, string value) : Attribute
{
    public string Key { get; } = key ?? throw new ArgumentNullException(nameof(key));

    public string Value { get; } = value ?? throw new ArgumentNullException(nameof(value));

    internal static readonly Dictionary<string, string> BuildInfoAttributes = typeof(BuildInfoAttribute)
        .Assembly.GetCustomAttributes(inherit: false)
        .OfType<BuildInfoAttribute>()
        .ToArray()
        .ToDictionary(o => o.Key, o => o.Value, StringComparer.OrdinalIgnoreCase);
}
