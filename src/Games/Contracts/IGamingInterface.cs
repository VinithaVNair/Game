

namespace Games.Contracts
{
    public interface IGamingInterface
    {
        void MadeAMove(string player, int value, int availableCards);
        void RoundCompleted(string winner);
        void GameCompleted(string winner);
    }
}
