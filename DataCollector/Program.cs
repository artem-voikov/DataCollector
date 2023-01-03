using DataCollector;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<TrackerSettings>(builder.Configuration);
builder.Services.AddSingleton<IDataCollectorService, DataCollectorService>();
builder.Services.AddSingleton<IConnectionFactory>(new ConnectionFactory { Endpoint = new AmqpTcpEndpoint(new Uri(builder.Configuration["TrackerSettings:ConnectionString"])) });
builder.Services.AddTransient(x => x.GetRequiredService<IConnectionFactory>().CreateConnection());
builder.Services.AddTransient(x => x.GetRequiredService<IConnection>().CreateModel());

var app = builder.Build();

app.MapGet("/track", async (HttpRequest request, [FromServices] IDataCollectorService dataCollector, CancellationToken cancellationToken)
    => await dataCollector.Collect(request, cancellationToken));

app.Run();