using Games.Contracts;
using Games.Core;
using Games.Implementations;
using System;
using System.Collections.Generic;

namespace GameApplication
{
    public class Start
    {
        static void Main(string[] args)
        {
            IGamingInterface gamingInterface = new GamingInterface();
            IShuffler<Card> shuffler = new Shuffler<Card>();
            IDeck<Card> cardDeck = new CardDeck();
            Random random = new Random();
            var playerNames = new List<string>() { "1", "2" };
            IList<IPlayer<Card>> players = new List<IPlayer<Card>>();
            playerNames.ForEach(v => players.Add(new Player<Card>(v, shuffler, random, new CardDeck(), new CardDeck())));

            var cardGame = new Game<Card>(gamingInterface, cardDeck, shuffler, players, 40, 10, random);

            cardGame.Play();
        }

    }
}
