using Microsoft.Extensions.Configuration;
using OutsourceTracker.Cli;

var app = new AppBuilder()
    .ConfigureAppConfiguration(confg => confg.AddJsonFile("appsettings.json"))
    .Build();

return app.Run(args);