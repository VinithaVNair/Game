using System.Collections.Generic;

namespace Games.Contracts
{
    public interface IPlayer<T>
    {
        /// <summary>
        /// Name of the Player
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Number of cards available on playing deck
        /// </summary>
        int DeckCardCount { get; }

        /// <summary>
        /// Number of cards added to discarded cards on winning rounds
        /// </summary>
        int DiscardedCardCount { get; }

        /// <summary>
        /// Cards available on playing deck
        /// </summary>
        IDeck<T> DeckCards { get; }

        /// <summary>
        /// Cards earned by winning rounds
        /// </summary>
        IDeck<T> DiscardedCards { get; }

        /// <summary>
        /// Shows the top (FIFO) card
        /// </summary>
        /// <returns></returns>
        T ShowCard();

        /// <summary>
        /// Use the cards added on winnig previous round for playing further
        /// </summary>
        void UseShuffledDiscardedSet();

        /// <summary>
        /// Add new set of cards to deck
        /// it would replace the existing deck if any
        /// </summary>
        /// <param name="card"></param>
        void AddDeckCard(IList<T> card);
        
        /// <summary>
        /// Add cards on winnig rounds
        /// </summary>
        /// <param name="cards"></param>
        void AddDiscardedCard(IList<T> cards);

        /// <summary>
        /// Total number cards available in hand
        /// </summary>
        /// <returns></returns>
        int GetTotalAvailableCardsCount();

    }
}