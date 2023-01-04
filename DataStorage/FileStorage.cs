using DataCollector;
using Microsoft.Extensions.Options;

namespace DataStorage
{
    public class FileStorage : IStorage
    {
        private readonly TrackerSettings trackerSettings;

        public FileStorage(IOptions<TrackerSettings> trackerSettings)
        {
            this.trackerSettings = trackerSettings.Value;
        }

        //TODO: make it test capable
        public async Task Log(string row) => await File.AppendAllTextAsync(trackerSettings.FileName, row);

    }
}
