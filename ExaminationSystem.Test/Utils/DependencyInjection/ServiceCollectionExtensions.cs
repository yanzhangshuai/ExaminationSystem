using EntityFrameworkCore.UnitOfWork.Extensions;
using ExaminationSystem.Application.Student;
using ExaminationSystem.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExaminationSystem.Test.Utils.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<DbContext, ExaminationSystemDbContext>();
        services.AddUnitOfWork();
        services.AddUnitOfWork<ExaminationSystemDbContext>();
        
        
        services.AddScoped<IStudentService, StudentService>();

        return services;
    }
}