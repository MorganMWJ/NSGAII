using Microsoft.Extensions.DependencyInjection;
using NSGAII.TestObjectiveFunctions;

namespace NSGAII;

internal class Program
{
    static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();

        ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var greetingService = serviceProvider.GetService<NSGAII>();
        greetingService?.Run();

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

        services.AddScoped<ITestProblem, FON>();
    }
}
