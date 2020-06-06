using Butler.Models;

namespace Butler.Interfaces
{
    public interface IGameProcessor
    {
        void ProcessGame(
            Game g,
            int num);
    }
}
