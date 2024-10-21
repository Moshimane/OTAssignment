
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
if (connectionString != null)
{
    builder.Services.AddScoped<IDbConnection>(x =>
    {
        var connection = new SqlConnection(connectionString);
        connection.Open();
        return connection;
    });
}
builder.Services.AddSingleton(MessageQueueConnectionHelper.GetInstance);
builder.Services.AddTransient<IMessagePublisher, MessagePublisher>();
builder.Services.AddTransient<IMessageSubscriber, MessageSubscriber>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckl
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        opts.EnableTryItOutByDefault();
        opts.DocumentTitle = "OT Assessment App";
        opts.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
