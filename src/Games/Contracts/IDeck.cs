using System;
using System.Collections.Generic;

namespace Games.Contracts
{
    public interface IDeck<T>
    {
        /// <summary>
        /// Cards available in the current deck
        /// </summary>
        Queue<T> Cards { get; }
        
        /// <summary>
        /// Shuffles the existing deck of cards
        /// </summary>
        /// <param name="cardShuffler"></param>
        /// <param name="random"></param>
        void Shuffle(IShuffler<T> cardShuffler, Random random);

        /// <summary>
        /// Adds Card to the deck
        /// </summary>
        /// <param name="card"></param>
        void AddCard(T card);

        /// <summary>
        /// Initialize the deck with new set of cards
        /// </summary>
        /// <param name="cards"></param>
        void SetDeck(IList<T> cards);

        /// <summary>
        /// Pick the top card from deck
        /// </summary>
        /// <returns></returns>
        T DrawTopCard();
        int Count { get; }
        void Reset();
        void CreateDeck(int totalCards, int maxCardValue);
    }
}