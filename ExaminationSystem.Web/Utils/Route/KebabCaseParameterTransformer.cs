using System.Text.RegularExpressions;

namespace ExaminationSystem.Web.Utils.Route;

public class KebabCaseParameterTransformer: IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        return value?.ToString()?.ToKebabCase();
    }
}