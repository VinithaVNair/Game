using Games.Contracts;
using System;

namespace Games.Implementations
{
    /// <summary>
    /// Shuffles a pile of items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Shuffler<T> : IShuffler<T>
    {
        #region Public methods
        public void Shuffle(T[] cards, Random random)
        {
            if (cards == null)
            {
                throw new ArgumentNullException("Nothing to Shuffle");
            }

            for (int i = cards.Length - 1; i >= 0; i--)
            {
                int j = random.Next(0, i + 1);

                Swap(i, j, cards);
            }
        }

        #endregion

        #region Private methods
        private void Swap(int i, int j, T[] cards)
        {
            T temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }

        #endregion
    }
}
