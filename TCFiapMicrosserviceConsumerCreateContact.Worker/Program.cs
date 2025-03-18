using MassTransit;
using TCFiapMicrosserviceConsumerCreateContact.Worker;
using TechChallenge.SDK;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        Task.Delay(15000).Wait();

        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_DATABASE") ?? 
        hostContext.Configuration.GetConnectionString("DefaultConnection");

        var envHostRabbitMqServer = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";

        services.RegisterSdkModule(connectionString);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateContactConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(envHostRabbitMqServer);

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