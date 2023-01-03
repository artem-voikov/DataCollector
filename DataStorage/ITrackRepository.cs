using System.Threading.Tasks;

namespace DataStorage
{
    public interface ITrackRepository
    {
        Task Store(string message);
    }
}