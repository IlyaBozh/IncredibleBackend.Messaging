using IncredibleBackendContracts.Abstractions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace IncredibleBackend.Messaging.Extentions;

public static class MessagingSetup
{
    public static void RegisterConsumersAndProducers(
        this IServiceCollection services,
        Action<IBusRegistrationConfigurator>? addConsumers,
        Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext>? configureConsumers,
        Action<IRabbitMqBusFactoryConfigurator>? configureProducers
    )
    {
        services.AddMassTransit(config =>
        {
            addConsumers?.Invoke(config);

            config.UsingRabbitMq((ctx, cfg) =>
            {
                configureConsumers?.Invoke(cfg, ctx);
                configureProducers?.Invoke(cfg);
            });
        });

    }

    public static void RegisterConsumer<T>(
        this IRabbitMqBusFactoryConfigurator cfg,
        IBusRegistrationContext ctx,
        string queueName
    ) where T : class, IConsumer
    {
        cfg.ReceiveEndpoint(queueName, c =>
        {
            c.ConfigureConsumer<T>(ctx);
        });
    }

    public static void RegisterProducer<T>(
        this IRabbitMqBusFactoryConfigurator cfg,
        string queueName
    ) where T : MessagingEvent
    {
        cfg.ReceiveEndpoint(queueName, c =>
        {
            c.Bind<T>();
        });
    }
}
