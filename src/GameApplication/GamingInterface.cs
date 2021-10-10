using Games.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameApplication
{
    public class GamingInterface : IGamingInterface
    {
        public void GameCompleted(string winner)
        {
            Console.WriteLine($"Player {winner} wins the game !");
        }

        public void MadeAMove(string player, int value, int availableCards)
        {
            Console.WriteLine($"Player {player}({availableCards} cards): {value}");

        }

        public void RoundCompleted(string winner)
        {
            Console.WriteLine($"Player {winner} wins this round");
        }
    }
}
