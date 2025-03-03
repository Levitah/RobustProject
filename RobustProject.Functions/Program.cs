using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using RobustProject.Services.DependencyInjection;
using RobustProject.Functions.DependencyInjection;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication((hostingContext, workerApplication) =>
    {

    })
    .ConfigureServices(services =>
    {
        services
            .AddFunctionDependencies()
            .AddServicesDependencies();
    })
    .Build();

await host.RunAsync();

//Below lines are added to ignore this class from Code Coverage
[ExcludeFromCodeCoverage]
public partial class Program { }