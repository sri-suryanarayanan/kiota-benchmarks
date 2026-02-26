namespace Aveva.Platform.EntityMgmt.Tests.Benchmarks.Extensions;

/// <summary>
/// Extensions for strings.
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// Returns empty if the string is null.
    /// </summary>
    internal static string EmptyIfNull(this string? value)
    {
        return value ?? string.Empty;
    }
}
