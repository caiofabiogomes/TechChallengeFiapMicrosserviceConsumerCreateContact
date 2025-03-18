using MassTransit;
using Microsoft.Extensions.Configuration;
using TCFiapMicrosserviceConsumerCreateContact.Worker;
using TechChallenge.SDK;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_DATABASE") ?? 
        hostContext.Configuration.GetConnectionString("DefaultConnection");

        services.RegisterSdkModule(connectionString);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateContactConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://localhost", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("create-contact-queue", e =>
                {
                    e.ConfigureConsumer<CreateContactConsumer>(context);
                });
            });
        });

        services.AddHostedService<Worker>();
    })
    .ConfigureLogging(logging =>
    {
        logging.SetMinimumLevel(LogLevel.Information);
    })
    .Build();

await host.RunAsync();