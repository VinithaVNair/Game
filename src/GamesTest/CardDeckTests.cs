using Games.Contracts;
using Games.Implementations;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GamesTest
{
    public class CardDeckTests
    {
        private CardDeck _cardDeck;

        [SetUp]
        public void Setup()
        {
            _cardDeck = new CardDeck();
        }

        [Test]
        public void CreateDeck_40totalCardWith10MAxValue_40CardsAddedToDeck()
        {
            //Arrange //Action
            _cardDeck.CreateDeck(40, 10);

            //Assert
            Assert.IsTrue(_cardDeck.Count == 40);
            Assert.IsTrue(_cardDeck.Cards.Count == 40);
        }

        [Test]
        public void CreateDeck_40totalCardWith10MAxValue_CardsFrom1To10ValueAreCreated()
        {
            //Arrange //Action
            _cardDeck.CreateDeck(40, 10);

            //Assert
            Assert.IsTrue(_cardDeck.Cards.Where(v => v.Value == 1).Count() == 4);
            Assert.IsTrue(_cardDeck.Cards.Where(v => v.Value == 2).Count() == 4);
            Assert.IsTrue(_cardDeck.Cards.Where(v => v.Value == 3).Count() == 4);
            Assert.IsTrue(_cardDeck.Cards.Where(v => v.Value == 4).Count() == 4);
            Assert.IsTrue(_cardDeck.Cards.Where(v => v.Value == 5).Count() == 4);
            Assert.IsTrue(_cardDeck.Cards.Where(v => v.Value == 6).Count() == 4);
            Assert.IsTrue(_cardDeck.Cards.Where(v => v.Value == 7).Count() == 4);
            Assert.IsTrue(_cardDeck.Cards.Where(v => v.Value == 8).Count() == 4);
            Assert.IsTrue(_cardDeck.Cards.Where(v => v.Value == 9).Count() == 4);
            Assert.IsTrue(_cardDeck.Cards.Where(v => v.Value == 10).Count() == 4);
        }

        [Test]
        public void SetDeck_NewCardSet_ReplaceDeckWithNewSet()
        {
            //Arrange
            _cardDeck.CreateDeck(40, 10);
            Assert.IsTrue(_cardDeck.Count == 40);

            //Action
            _cardDeck.SetDeck(new List<Card>() { new Card(1) });

            //Assert
            Assert.IsTrue(_cardDeck.Count == 1);
        }

        [Test]
        public void SetDeck_Null_ThrowsNullException()
        {
            //Arrange //Action //Assert
            Assert.Throws<ArgumentNullException>(() => _cardDeck.SetDeck(null));
        }

        [Test]
        public void AddCard_NewCard_CardsCountIncreased()
        {
            //Arrange
            _cardDeck.SetDeck(new List<Card>() { new Card(1) });
            Assert.IsTrue(_cardDeck.Count == 1);

            //Action
            _cardDeck.AddCard(new Card(2));

            //Assert
            Assert.IsTrue(_cardDeck.Count == 2);
        }

        [Test]
        public void AddCard_Null_ThrowsNullException()
        {
            //Arrange //Action
            Assert.Throws<ArgumentNullException>(() => _cardDeck.AddCard(null));
        }

        [Test]
        public void DrawTopCard_OrderdCards_ReturnsFirstCard()
        {
            //Arrange
            var card1 = new Card(1);
            var card2 = new Card(2);
            _cardDeck.SetDeck(new List<Card>() { card1, card2 });

            //Action
            var card = _cardDeck.DrawTopCard();

            //Assert
            Assert.AreEqual(card1, card);
        }

        [Test]
        public void Shuffle_OrderdCards_CardsAreShuffled()
        {
            //Arrange
            var shuffler = new Mock<IShuffler<Card>>();
            var card1 = new Card(1);
            var card2 = new Card(2);
            _cardDeck.SetDeck(new List<Card>() { card1, card2 });

            //Action
            _cardDeck.Shuffle(shuffler.Object, new Random());

            //Assert
            shuffler.Verify(v => v.Shuffle(It.Is<Card[]>(v => v.Length == 2), It.IsAny<Random>()));
        }
    }
}
