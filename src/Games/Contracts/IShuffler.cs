using System;

namespace Games.Contracts
{
    public interface IShuffler<T>
    {
        /// <summary>
        /// Shuffle a pile items
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="random"></param>
        void Shuffle(T[] cards, Random random);
    }
}
