using DataStorage;
using System.Text.Json;

public static class MyExtensions
{
    public static string PrintIfEmpty(this string target, string value)
        => string.IsNullOrWhiteSpace(target) ? value : target;
}

public class TrackRepository : ITrackRepository
{
    private readonly IStorage storage;

    public TrackRepository(IStorage storage)
    {
        this.storage = storage;
    }

    public async Task Store(string message)
    {
        var request = JsonSerializer.Deserialize<TrackRequest>(message);
        var row = $"{request.UtcNow}|{request.Referer.PrintIfEmpty("null")}|{request.UserAgent.PrintIfEmpty("null")}|{request.Ip}";
        await storage.Log($"{row}\n");
    }
}