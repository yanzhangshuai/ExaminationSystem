namespace ExaminationSystem.Web.Utils.Exceptions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomException(this IServiceCollection services)
    {
        // 注册自定义异常处理
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddExceptionHandler<SystemExceptionHandle>();

        return services;
    }
}