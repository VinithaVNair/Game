using Games.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Games.Implementations
{
    /// <summary>
    /// Player can play any type of card game
    /// Deck would be used for making moves while playing
    /// DiscardedCard would have all the cards collected on 
    /// winning rounds
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Player<T> : IPlayer<T>
    {
        #region Private properties
        private readonly IShuffler<T> _cardShuffler;
        private readonly Random _random;
        #endregion

        #region Public methods

        public string Name { get; }
        public IDeck<T> DeckCards { get; }
        public IDeck<T> DiscardedCards { get; }
        public int DeckCardCount => DeckCards.Count;
        public int DiscardedCardCount => DiscardedCards.Count;

        public Player(string name, IShuffler<T> cardShufller, Random random, IDeck<T> discardedCards, IDeck<T> deckCards)
        {
            _cardShuffler = cardShufller;
            _random = random;
            Name = name;
            DiscardedCards = discardedCards;
            DeckCards = deckCards;
        }

        public T ShowCard()
        {
            if (!DeckCards.Cards.Any())
            {
                throw new InvalidOperationException("Player has no card to show");
            }
            return DeckCards.DrawTopCard();
        }


        public void AddDeckCard(IList<T> cards)
        {
            if (cards == null)
            {
                throw new ArgumentNullException("Null Cards can not be added to deck card");
            }

            foreach (var card in cards)
            {
                DeckCards.AddCard(card);
            }
        }

        public void AddDiscardedCard(IList<T> cards)
        {
            if (cards == null)
            {
                throw new ArgumentNullException("Null Cards can not be added to discarded card");
            }

            foreach (var card in cards)
            {
                DiscardedCards.AddCard(card);
            }
        }

        public int GetTotalAvailableCardsCount()
        {
            return DiscardedCards.Count + DeckCards.Count;
        }

        public void UseShuffledDiscardedSet()
        {
            if (DiscardedCards.Cards.Any())
            {
                DeckCards.SetDeck(DiscardedCards.Cards.ToList());
                DeckCards.Shuffle(_cardShuffler, _random);
                DiscardedCards.Reset();
            }
        }
        #endregion
    }
}
