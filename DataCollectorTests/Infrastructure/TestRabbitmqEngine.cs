using DataCollector;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace DataCollectorTests.Infrastructure
{
    internal class TestRabbitEngine : IDisposable
    {
        private TrackerSettings settings;
        private IConnection connection;
        private IModel channel;
        public string amqpConnectionString;
        private IConfigurationRoot config;

        public TestRabbitEngine()
        {
            var configBuilder = new ConfigurationBuilder();
            config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Tests.json") //do not commit this guy
                .AddEnvironmentVariables()
                .Build();
            config.GetSection("RabbitMq").Bind(settings);

            amqpConnectionString = config["TrackerSettings:ConnectionString"];

            CreateChannel(amqpConnectionString);
        }

        public TestRabbitEngine(string amqpConnectionString)
            => CreateChannel(amqpConnectionString);

        public void Dispose()
        {
            channel.Dispose();
            connection.Dispose();
        }

        public TrackerSettings Settings { get => settings; }
        public TestRabbitEngine CreateChannel(string? uri)
        {
            var factory = new ConnectionFactory()
            {
                Endpoint = new AmqpTcpEndpoint(new Uri(uri ?? amqpConnectionString ?? settings.ConnectionString))
            };

            connection = factory.CreateConnection();

            channel = connection.CreateModel();

            return this;
        }

        public void DeleteQueue(string queueName)
            => channel.QueueDelete(queueName);

        public void CreateQueue(string queueName)
            => channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        public void Publish(string queueName, string message)
            => channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(message));

        public void Consume(string queueName, CancellationToken cts, EventHandler<BasicDeliverEventArgs> action)
        {
            Task.Run(() =>
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += action;
                channel.BasicConsume(queue: queueName, autoAck: false, consumer);

                while (!cts.IsCancellationRequested)
                    Thread.Sleep(1000);

            }, cts);
        }

    }
}
