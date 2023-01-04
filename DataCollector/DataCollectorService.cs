using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace DataCollector
{
    public class DataCollectorService : IDataCollectorService
    {
        private readonly IModel _channel;
        private readonly TrackerSettings settings;

        public DataCollectorService(IModel channel, IOptions<TrackerSettings> settings)
        {
            this._channel = channel;
            this.settings = settings.Value;
        }

        public async Task<string> Collect(HttpRequest request, CancellationToken cancellationToken)
        {
            var ip = request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            var referer = request.Headers["referer"].ToString();
            var userAgent = request.Headers["User-Agent"].ToString();
            var utcNow = DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture);

            var result = await SendMessage(new { ip, referer, userAgent, utcNow }, cancellationToken);

            return result;
        }

        //TODO: make it test capable
        private async Task<string> SendMessage(object value, CancellationToken cancellationToken)
        {
            var body = JsonSerializer.Serialize(value);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.CorrelationId = Guid.NewGuid().ToString();

            _channel.BasicPublish(exchange: string.Empty, settings.QueueName, basicProperties: properties, body: Encoding.UTF8.GetBytes(body));

            return body;
        }
    }

    public interface IDataCollectorService
    {
        Task<string> Collect(HttpRequest request, CancellationToken cancellationToken);
    }
}
