
var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    })
    .ConfigureServices((context, services) =>
    {
        //configure services
        services.AddScoped<IDbConnection>(x =>
        {
            var connection = new SqlConnection(context.Configuration.GetConnectionString("DatabaseConnection"));
            connection.Open();
            return connection;
        });
        services.AddSingleton(MessageQueueConnectionHelper.GetInstance);
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();

        services.AddHostedService<MessageSubscriberService>();

    })
    .Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started {time:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

await host.RunAsync();

logger.LogInformation("Application ended {time:yyyy-MM-dd HH:mm:ss}", DateTime.Now);