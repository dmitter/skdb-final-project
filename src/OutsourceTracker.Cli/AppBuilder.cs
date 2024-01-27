using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace OutsourceTracker.Cli;

public class AppBuilder
{
    private readonly IServiceCollection _registrations = new ServiceCollection();
    private readonly IConfigurationBuilder _configurationBuilder = new ConfigurationBuilder();
    private Action<IServiceCollection>? _configureServicesDelegate;
    private Action<IConfigurationBuilder>? _configureAppConfigurationDelegate;

    public readonly Action<IConfigurator> Configuration = config =>
    {
        config.SetApplicationName("outsource-tracker");

        config.AddCommand<ImportCommand>("import")
            .WithExample("import", "positions.csv")
            .WithExample("import", "employees.csv")
            .WithExample("import", "timesheet.csv");

        config.AddCommand<ListCommand>("list")
            .WithExample("list", "employee");

        config.AddCommand<GetCommand>("get");

        config.AddCommand<RemoveCommand>("remove");

        config.AddCommand<ReportCommand>("report")
            .WithExample("report", "top5longTasks")
            .WithExample("report", "top5costTasks")
            .WithExample("report", "top5employees");
    };

    public TypeRegistrar? Registrar { get; private set; }

    public CommandApp Build()
    {
        var app = GetApp();
        app.Configure(Configuration);
        return app;
    }

    public AppBuilder ConfigureServices(Action<IServiceCollection> configureDelegate)
    {
        _configureServicesDelegate = configureDelegate;
        return this;
    }

    public AppBuilder ConfigureAppConfiguration(Action<IConfigurationBuilder> configureDelegate)
    {
        _configureAppConfigurationDelegate = configureDelegate;
        return this;
    }

    private CommandApp GetApp()
    {
        var configuration = GetConfiguration();
        Registrar = GetTypeRegistrar(configuration);
        return new CommandApp(Registrar);
    }

    private TypeRegistrar GetTypeRegistrar(IConfiguration configuration)
    {
        _registrations.AddSingleton(configuration[Config.ConnectionString] ?? string.Empty);
        _configureServicesDelegate?.Invoke(_registrations);
        return new TypeRegistrar(_registrations);
    }

    private IConfiguration GetConfiguration()
    {
        _configureAppConfigurationDelegate?.Invoke(_configurationBuilder);
        return _configurationBuilder.Build();
    }
}