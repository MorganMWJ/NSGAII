using Microsoft.Extensions.DependencyInjection;
using NSGAII.Factories;
using NSGAII.TestObjectiveFunctions;

namespace NSGAII;

internal class Program
{
    static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();

        ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var nsgaii = serviceProvider.GetService<NSGAII>();
        nsgaii?.Run();

        if (serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }

        Console.ReadKey();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<NSGAII>();
        services.AddScoped<NSGAIIOptions>();
        services.AddScoped<Population>();
        services.AddScoped<Solution>();

        services.AddScoped<ISolutionFactory, SolutionFactory>();
        services.AddScoped<IPopulationFactory, PopulationFactory>();
        services.AddScoped<ITestProblem, FON>();
    }
}
