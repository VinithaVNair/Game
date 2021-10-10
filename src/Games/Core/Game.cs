
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Games.Contracts;

namespace Games.Core
{
    /// <summary>
    /// A Game which can be played between several players
    /// where players will be assigned with equal number cards
    /// every turn based on the score winner of the round will take all the cards
    /// A player can be a winner in two situation 
    /// 1.when s/he has all the cards
    /// 2.One of the player has no more card left to play then player with max number
    /// of card will be winner
    /// </summary>
    /// <typeparam name="T">any type of card</typeparam>

    public class Game<T> : IGame
        where T : IComparable, ICard
    {
        #region Private properties

        private readonly IGamingInterface _gamingInterface;
        private readonly IList<IPlayer<T>> _players;
        private readonly IDeck<T> _cardDeck;
        #endregion

        #region Public methods

        public Game(IGamingInterface gamingInterface, IDeck<T> cardDeck, IShuffler<T> cardShuffler,
            IList<IPlayer<T>> players, int totalCards, int maxCardValue, Random random)
        {
            ValidateInput(gamingInterface, cardDeck, cardShuffler, players, totalCards, maxCardValue);

            _gamingInterface = gamingInterface;
            _cardDeck = cardDeck;
            _players = players;

            _cardDeck.CreateDeck(totalCards, maxCardValue);
            _cardDeck.Shuffle(cardShuffler, random);

            SetEachPlayersDeckWithEqualNumberOfCards();
        }

        public void Play()
        {
            IList<T> playedCards = new List<T>();
            bool playGame = true;

            while (playGame)
            {
                var moves = new Dictionary<T, IPlayer<T>>();
                foreach (var player in _players)
                {
                    if (player.GetTotalAvailableCardsCount() == 0)
                    {
                        var gameWinner = GetPlayerWithMaximumCards();
                        _gamingInterface.GameCompleted(gameWinner.Name);
                        playGame = false;
                        break;
                    }
                    else if (player.DeckCardCount == 0)
                    {
                        player.UseShuffledDiscardedSet();
                    }

                    var move = player.ShowCard();
                    moves.Add(move, player);
                    playedCards.Add(move);

                    _gamingInterface.MadeAMove(player.Name, move.Value, player.DeckCardCount);
                }
                var winner = GetWinner(moves);

                if (winner != null)//if there is no winner then the draw continues
                {
                    _gamingInterface.RoundCompleted(winner.Name);
                    winner.AddDiscardedCard(playedCards);
                    playedCards = new List<T>();

                    if (winner.GetTotalAvailableCardsCount() == _cardDeck.Count)
                    {
                        _gamingInterface.GameCompleted(winner.Name);
                        playGame = false;
                    }
                }
            }

        }

        #endregion

        #region Private methods
        private void ValidateInput(IGamingInterface gamingInterface, IDeck<T> cardDeck, IShuffler<T> cardShuffler, IList<IPlayer<T>> players, int totalCards, int maxCardValue)
        {
            if (gamingInterface == null)
            {
                throw new ArgumentNullException("Gaming Interface shouldn't be null");
            }
            if (cardDeck == null)
            {
                throw new ArgumentNullException("Card Deck shouldn't be null");
            }
            if (cardShuffler == null)
            {
                throw new ArgumentNullException("Card Shuffler shouldn't be null");
            }
            if (players == null)
            {
                throw new ArgumentNullException("Players shouldn't be null");
            }
            if (players.Count() <= 1)
            {
                throw new ArgumentException("More than one player is required in this game");
            }
            if (totalCards < 2)
            {
                throw new ArgumentException("Atleast 2 cards are needed for playing the game");
            }
            if (maxCardValue < 2)
            {
                throw new ArgumentException("MaxCardValue can't be less than 2");
            }
        }

        private void SetEachPlayersDeckWithEqualNumberOfCards()
        {
            if (_cardDeck.Count <= 0 || _players.Count() <= 0)
            {
                return;
            }

            int cardsPerPlayer = _cardDeck.Count / _players.Count();
            var allCards = new Queue<T>(_cardDeck.Cards);
            int playerIndex = 0;
            IList<T> cards = new List<T>();
            int numberOfCards = 0;

            for (int i = 0; i < _cardDeck.Count; i++)
            {
                cards.Add(allCards.Dequeue());
                numberOfCards++;
                if (cardsPerPlayer == numberOfCards)
                {
                    _players[playerIndex].DeckCards.SetDeck(cards);
                    cards = new List<T>();
                    numberOfCards = 0;
                    playerIndex++;
                }
            }

            //foreach (var player in _players)
            //{
            //    player.DeckCards.SetDeck(totalCards.Take(n).ToList());
            //    totalCards = totalCards.Skip(n).ToList();
            //}
        }

        private IPlayer<T> GetPlayerWithMaximumCards()
        {
            return _players.OrderByDescending(v => v.GetTotalAvailableCardsCount()).First();
        }

        private IPlayer<T> GetWinner(IDictionary<T, IPlayer<T>> moves)
        {
            if (!moves.Any())
            {
                return null;
            }
            IPlayer<T> winner = moves.First().Value;
            T max = moves.First().Key;

            foreach (var move in moves.Skip(1))
            {
                var compared = max.CompareTo(move.Key);
                if (compared == 0)
                {
                    return null;
                }
                if (compared < 0)
                {
                    max = move.Key;
                    winner = move.Value;
                }
            }
            return winner;
        }

        #endregion
    }
}
