using Games.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Games.Implementations
{
    /// <summary>
    /// Deck for type Card
    /// Pile of card which can be shuffled or changed
    /// top card of the pile can be drawn for making a move
    /// </summary>
    public class CardDeck : IDeck<Card>
    {
        #region Private properties
        private Queue<Card> _cards = new Queue<Card>();
        #endregion

        #region Public methods
        public Queue<Card> Cards => _cards;
        public int Count => _cards.Count();
        public void CreateDeck(int totalCards, int maxCardValue)
        {
            IList<Card> cards = new List<Card>();
            for (int i = 0; i < totalCards; i++)
            {
                Card card = new Card(i % maxCardValue + 1);
                cards.Add(card);
            }
            SetDeck(cards);
        }

        public void SetDeck(IList<Card> cards)
        {
            if (cards == null)
            {
                throw new ArgumentNullException("Cards can not be null");
            }

            _cards = new Queue<Card>(cards);
        }

        public void AddCard(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException("Card can not be null");
            }
            _cards.Enqueue(card);
        }

        public Card DrawTopCard()
        {
            return _cards.Any() ? _cards.Dequeue() : null;
        }

        public void Shuffle(IShuffler<Card> cardShuffler, Random random)
        {
            var cards = _cards.ToArray();
            cardShuffler.Shuffle(cards, random);
            _cards = new Queue<Card>(cards);
        }

        public void Reset()
        {
            _cards = new Queue<Card>();
        }

        #endregion


    }
}
