namespace DataCollector
{
    public class TrackerSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
        public string ResponseQueueName { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
