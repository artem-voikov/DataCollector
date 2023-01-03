using DataCollector;
using DataStorage;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<TrackerSettings>(builder.Configuration);
builder.Services.AddSingleton<ITrackRepository, TrackRepository>();
builder.Services.AddSingleton<IStorage, FileStorage>();

builder.Services.AddSingleton<IConnectionFactory>(new ConnectionFactory { Endpoint = new AmqpTcpEndpoint(new Uri(builder.Configuration["TrackerSettings:ConnectionString"])) });
builder.Services.AddTransient(x => x.GetRequiredService<IConnectionFactory>().CreateConnection());
builder.Services.AddTransient(x => x.GetRequiredService<IConnection>().CreateModel());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Add consumer
var channel = app.Services.GetService<IModel>();
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, args) =>
    app.Services.GetService<ITrackRepository>()?.Store(
        Encoding.UTF8.GetString(args.Body.ToArray())
        );

app.Run();
