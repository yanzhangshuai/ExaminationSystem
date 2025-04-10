using System.Text.RegularExpressions;

namespace ExaminationSystem.Web.Utils;

//TODO: 此类应当在基础设施层
public static class StringExtensions
{
    public static string ToKebabCase(this string? input)
    {
        return Regex.Replace(input, "([a-z])([A-Z])", "$1-$2").ToLowerInvariant();
    }
}