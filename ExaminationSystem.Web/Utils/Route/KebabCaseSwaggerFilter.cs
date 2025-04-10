using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ExaminationSystem.Web.Utils.Route;

public class KebabCaseSwaggerFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = new OpenApiPaths();
        foreach (var (key, value) in swaggerDoc.Paths)
        {
            // 转换Path中的占位符和固定段
            paths.Add(key.ToKebabCase(), value); 
        }
        swaggerDoc.Paths = paths;

    }
}
