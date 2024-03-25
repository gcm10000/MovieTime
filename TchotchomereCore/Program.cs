using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using TchotchomereCore;
using TchotchomereCore.Application;
using TchotchomereCore.Infrastructure.Sql.IoC;
using TchotchomereCore.Infrastructure.TeuTorrent.IoC;
using TchotchomereCore.Infrastructure.Queue.IoC;
using TchotchomereCore.Infrastructure.MovieDB.IoC;

static string GetEnvironment()
{
    if (Debugger.IsAttached)
        return "Development";

    return "Production";
}


var host = CreateHostBuilder(args).Build();

host.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            var currentFileName = typeof(Program).Assembly.Location;

            var pathcurrentDirectory = Path.GetDirectoryName(currentFileName);

            IConfiguration configuration = new ConfigurationBuilder()
               .SetBasePath(pathcurrentDirectory!)
               .AddJsonFile("appSettings.json", false)
               .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddHostedService<Worker>();

            services.ConfigureApplication();
            services.ConfigureSql(configuration);
            services.ConfigureQueue(configuration);
            services.ConfigureTeuTorrent();
            services.ConfigureMovieDB(configuration);

            services.AddLogging();
        })
        .UseEnvironment(GetEnvironment())
        .ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Information);
        });