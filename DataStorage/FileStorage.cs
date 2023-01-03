using DataCollector;

namespace DataStorage
{
    public class FileStorage : IStorage
    {
        private readonly TrackerSettings trackerSettings;

        public FileStorage(TrackerSettings trackerSettings)
        {
            this.trackerSettings = trackerSettings;
        }

        public async Task Log(string row) => await File.AppendAllTextAsync(trackerSettings.FileName, row);

    }
}
