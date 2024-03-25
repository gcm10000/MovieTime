using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TchotchomereCore.Domain.Interfaces;
using TchotchomereCore.Infrastructure.Queue.Services;

namespace TchotchomereCore.Infrastructure.Queue.IoC;

public static class IoCExtensions
{
    public static IServiceCollection ConfigureQueue(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDomainPublisherToOutbox, QueuePublisherService>();

        var rabbitMQOptions = configuration.GetRequiredSection(nameof(RabbitMQOptions));

        services.AddCap(x =>
        {
            var connectionString = configuration.GetConnectionString("CAP");
            
            x.UseMySql(connectionString!);
            
            x.UseRabbitMQ(configure =>
            {
                configure.UserName = rabbitMQOptions[nameof(RabbitMQOptions.UserName)]!;
                configure.Password = rabbitMQOptions[nameof(RabbitMQOptions.Password)]!;
                configure.HostName = rabbitMQOptions[nameof(RabbitMQOptions.HostName)]!;
            });
        });

        return services;
    }
}
