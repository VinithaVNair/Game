using Games.Contracts;
using System;

namespace Games.Implementations
{
    public class Card : ICard, IComparable
    {
        #region Public methods
        public int Value { get; }
        public Card(int value)
        {
            Value = value;
        }

        public int CompareTo(object obj)
        {
            var other = (Card)obj;
            int comparedValue = 0;
            if (Value < other.Value)
            {
                comparedValue = -1;
            }
            else if (Value > other.Value)
            {
                comparedValue = 1;
            }
            return comparedValue;
        }
        #endregion

    }
}
