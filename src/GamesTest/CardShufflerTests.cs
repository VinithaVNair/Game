using System;
using NUnit.Framework;
using Games.Contracts;
using Games.Implementations;

namespace GamesTest
{
    public class CardShufflerTests
    {
        private IShuffler<Card> _shuffler;

        [Test]
        public void Shuffle_Null_ThrowsNullException()
        {
            //Arrange //Action
            _shuffler = new Shuffler<Card>();

            //Assert
            Assert.Throws<ArgumentNullException>(() => _shuffler.Shuffle(null, new RandomIndex()));
        }

        [Test]
        public void Shuffle_OrderedInput_OrderIsShuffled()
        {
            //Arrange
            _shuffler = new Shuffler<Card>();

            Card[] cards = new Card[] { new Card(1), new Card(2), new Card(3), new Card(4) };
            Card[] expectedShuffledCard = new Card[] { new Card(4), new Card(3), new Card(2), new Card(1) };

            //Action
            _shuffler.Shuffle(cards, new RandomIndex());

            //Assert
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(expectedShuffledCard[i].Value, cards[i].Value);
            }
        }

        private class RandomIndex : Random
        {
            int i = 0;
            public override int Next(int min, int max)
            {
                return i >= max ? max - 1 : i++;
            }
        }
    }
}
