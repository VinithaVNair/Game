using Games.Contracts;
using Games.Implementations;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GamesTest
{
    public class PlayerTests
    {
        private Player<Card> _cardPlayer;
        private Mock<IDeck<Card>> _deck;
        private Mock<IDeck<Card>> _discardedDeck;
        private Mock<IShuffler<Card>> _shuffler;
        private Mock<IGamingInterface> _gamingInterface;

        [SetUp]
        public void Setup()
        {
            _deck = new Mock<IDeck<Card>>();
            _discardedDeck = new Mock<IDeck<Card>>();
            _shuffler = new Mock<IShuffler<Card>>();
            _gamingInterface = new Mock<IGamingInterface>();
            _cardPlayer = new Player<Card>("1", _shuffler.Object, new Random(), _discardedDeck.Object, _deck.Object);
        }



        [Test]
        public void UseDiscardedSet_WonCardOnDiscardedSet_SetDeckWithDiscardedCards()
        {
            //Arrange
            var cards = new List<Card> { new Card(1), new Card(2) };
            _discardedDeck.Setup(v => v.Cards).Returns(new Queue<Card>(cards));

            //Action
            _cardPlayer.UseShuffledDiscardedSet();

            //Assert
            _deck.Verify(v => v.SetDeck(It.Is<IList<Card>>(v => v.Count == 2)));
            _discardedDeck.Verify(v => v.Reset());
        }

        [Test]
        public void UseDiscardedSet_WonCardOnDiscardedSet_DiscardedSetShouldBeShuffled()
        {
            //Arrange
            var cards = new List<Card> { new Card(1), new Card(2) };
            _discardedDeck.Setup(v => v.Cards).Returns(new Queue<Card>(cards));
            var random = new Random();
            _cardPlayer = new Player<Card>("1", _shuffler.Object, random, _discardedDeck.Object, _deck.Object);

            //Action
            _cardPlayer.UseShuffledDiscardedSet();

            //Assert
            _deck.Verify(v => v.Shuffle(_shuffler.Object, random));
        }

        [Test]
        public void UseDiscardedSet_WithNoDiscardedSet_NothingShouldHappen()
        {
            //Arrange
            _discardedDeck.Setup(v => v.Cards).Returns(new Queue<Card>());

            var random = new Random();
            _cardPlayer = new Player<Card>("1", _shuffler.Object, random, _discardedDeck.Object, _deck.Object);

            //Action
            _cardPlayer.UseShuffledDiscardedSet();

            //Assert
            _deck.Verify(v => v.SetDeck(It.Is<IList<Card>>(v => v.Count == 2)), Times.Never);
            _discardedDeck.Verify(v => v.Reset(), Times.Never);
            _deck.Verify(v => v.Shuffle(_shuffler.Object, random), Times.Never);
        }

        [Test]
        public void GetCardsCount_2Discarded2DeckCard_Returns4()
        {
            //Arrange
            _deck.Setup(v => v.Count).Returns(2);
            _discardedDeck.Setup(v => v.Count).Returns(2);

            //Action
            var count = _cardPlayer.GetTotalAvailableCardsCount();

            //Assert
            Assert.AreEqual(4, count);
        }

        [Test]
        public void AddDiscardedCard_null_ThrowsNullException()
        {
            //Arrange //Action //Assert
            Assert.Throws<ArgumentNullException>(() => _cardPlayer.AddDiscardedCard(null));
        }

        [Test]
        public void AddDeckCard_null_ThrowsNullException()
        {
            //Arrange //Action //Assert
            Assert.Throws<ArgumentNullException>(() => _cardPlayer.AddDeckCard(null));
        }

        [Test]
        public void ShowCard_EmptyDeckCard_ThrowsInvalidOperationExceptionException()
        {
            //Arrange
            _deck.Setup(v => v.Cards).Returns(new Queue<Card>());

            //Action
            Assert.Throws<InvalidOperationException>(() => _cardPlayer.ShowCard());
        }

        [Test]
        public void AddDiscardedCard_NewSetOfCard_CardsAreAdded()
        {
            //Arrange
            var cards = new List<Card> { new Card(1), new Card(2) };

            //Action
            _cardPlayer.AddDiscardedCard(cards);

            //Assert
            foreach (var card in cards)
            {
                _discardedDeck.Verify(v => v.AddCard(card));
            }

        }

        [Test]
        public void AddDeckCard_NewSetOfCard_CardsAreAdded()
        {
            //Arrange
            var cards = new List<Card> { new Card(1), new Card(2) };

            //Action
            _cardPlayer.AddDeckCard(cards);

            //Assert
            foreach (var card in cards)
            {
                _deck.Verify(v => v.AddCard(card));
            }
        }

        [Test]
        public void ShowCard_OrderdCards_ReturnsFirstCard()
        {
            //Arrange
            var cards = new List<Card> { new Card(1), new Card(2) };
            _deck.Setup(v => v.Cards).Returns(new Queue<Card>(cards));

            //Action
            _cardPlayer.ShowCard();

            //Assert
            _deck.Verify(v => v.DrawTopCard());
        }
    }
}
