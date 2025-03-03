using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RobustProject.Services.Entities;
using RobustProject.Services.Options;
using RobustProject.Services.Repositories;
using RobustProject.Services.Repositories.SqlServer;
using RobustProject.Services.Repositories.SqlServer.EntityConfiguration;
using RobustProject.Services.Services.RosebudService;
using RobustProject.Services.Validation;
using System.Reflection;

namespace RobustProject.Services.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddServicesDependencies(this IServiceCollection services)
    {
        return services
            .AddMapper()
            .AddServiceImplementations()
            .AddSqlServer()
            .AddRepositories()
            .AddValidation();
    }

    public static IServiceCollection AddSettingOptions<T>(this IServiceCollection services, string? section = null) where T : class
    {
        services.AddOptions<T>().Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.Bind(settings);

            if (!string.IsNullOrEmpty(section))
                configuration.GetSection(section).Bind(settings);
        });

        return services;
    }

    public static T GetOptions<T>(this IServiceCollection services) where T : class
    {
        return ((IOptions<T>)services.BuildServiceProvider().GetService(typeof(IOptions<T>))!).Value;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        return services.AddAutoMapper(new List<Assembly> { Assembly.GetExecutingAssembly() });
    }

    private static IServiceCollection AddServiceImplementations(this IServiceCollection services)
    {
        return services
            .AddScoped<IRosebudService, RosebudService>();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped(typeof(IEntityRepository<,>), typeof(EntityRepository<,>))
            .AddScoped<IRosebudRepository, RosebudRepository>();
    }

    #region SqlServerDb
    public static IServiceCollection AddSqlServer(this IServiceCollection services)
    {
        services.AddSettingOptions<SqlServerOptions>(SqlServerOptions.Section);
        var sqlServerDbOptions = services.GetOptions<SqlServerOptions>();

        services.AddDbContext<SqlServerDbContext>(options => options.UseSqlServer(sqlServerDbOptions.ConnectionString!, sqlServerOptions =>
        {
            sqlServerOptions.ExecutionStrategy(esd => new ResilienceExecutionStrategy(esd, sqlServerDbOptions.ClientRetries, sqlServerDbOptions.ClientRetryDelayInMilliseconds));
        }));
        services.AddScoped(typeof(DbContext), typeof(SqlServerDbContext));

        services.AddScoped<IEntityTypeConfiguration<Rosebud>, RosebudEntityConfiguration>();

        return services;
    }
    #endregion

    #region Validation
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        return services.AddScoped<IModelValidator, ModelValidator>();
    }
    #endregion
}