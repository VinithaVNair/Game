using Games.Contracts;
using Games.Core;
using Games.Implementations;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GamesTest
{
    public class CardGameTests
    {
        private IGame _game;
        private Mock<IPlayer<Card>> _player1;
        private Mock<IPlayer<Card>> _player2;

        [SetUp]
        public void Setup()
        {
            _player1 = new Mock<IPlayer<Card>>();
            _player1.Setup(v => v.Name).Returns("1"); 
            _player1.Setup(v => v.DeckCards).Returns(new Mock<IDeck<Card>>().Object);

            _player2 = new Mock<IPlayer<Card>>();
            _player2.Setup(v => v.Name).Returns("2");
           _player2.Setup(v => v.DeckCards).Returns(new Mock<IDeck<Card>>().Object);

        }

        [Test]
        public void Game_Initialize_DeckHas40Cards()
        {
            //Arrange
            var deck = new Mock<IDeck<Card>>();
            var shuffler = new Mock<IShuffler<Card>>();
            var gamingInterface = new Mock<IGamingInterface>();

            //Action
            IList<IPlayer<Card>> players = new List<IPlayer<Card>>() { _player1.Object, _player2.Object };
            _game = new Game<Card>(gamingInterface.Object, deck.Object, shuffler.Object, players, 40, 10, new System.Random());

            //Assert
            deck.Verify(v => v.CreateDeck(40, 10), Times.Once);
        }

        [Test]
        public void Game_Initialize_DeckShouldBeShuffled()
        {
            //Arrange
            var deck = new Mock<IDeck<Card>>();
            var cards = new List<Card> { new Card(1), new Card(2) };
            deck.Setup(v => v.Cards).Returns(new Queue<Card>(cards));
            deck.Setup(v => v.Count).Returns(2);
            var shuffler = new Mock<IShuffler<Card>>();
            var gamingInterface = new Mock<IGamingInterface>();
            IList<IPlayer<Card>> players = new List<IPlayer<Card>>() { _player1.Object, _player2.Object };
           
            //Action
            _game = new Game<Card>(gamingInterface.Object, deck.Object, shuffler.Object, players, 2, 2, new System.Random());

            //Assert
            deck.Verify(v => v.Shuffle(It.IsAny<IShuffler<Card>>(), It.IsAny<System.Random>()), Times.Once);
        }

        [Test]
        public void Game_PlayWithEmptyDrawPile_UseAndShuffleDiscardedSet()
        {
            //Arrange
            var deck = new Mock<IDeck<Card>>();
            var cards = new List<Card> { new Card(1), new Card(2) };
            deck.Setup(v => v.Cards).Returns(new Queue<Card>(cards));
            deck.Setup(v => v.Count).Returns(2);
            var shuffler = new Mock<IShuffler<Card>>();
            var gamingInterface = new Mock<IGamingInterface>();

            Queue<Card> firstQueue = new Queue<Card>(new List<Card>() { new Card(1) });
            _player1.Setup(v => v.ShowCard()).Returns(firstQueue.Dequeue);
            _player1.Setup(v => v.DiscardedCardCount).Returns(2);
            _player1.Setup(v => v.DeckCardCount).Returns(0);
            _player1.Setup(v => v.GetTotalAvailableCardsCount()).Returns(2);

            Queue<Card> SecondQueue = new Queue<Card>(new List<Card>() { new Card(2) });
            _player2.Setup(v => v.ShowCard()).Returns(SecondQueue.Dequeue);
            _player2.Setup(v => v.DiscardedCardCount).Returns(2);
            _player2.Setup(v => v.DeckCardCount).Returns(0);
            _player2.Setup(v => v.GetTotalAvailableCardsCount()).Returns(2);

            IList<IPlayer<Card>> players = new List<IPlayer<Card>>() { _player1.Object, _player2.Object };
            _game = new Game<Card>(gamingInterface.Object, deck.Object, shuffler.Object, players, 2, 2, new System.Random());

            //Action
            _game.Play();

            //Assert
            _player1.Verify(v => v.UseShuffledDiscardedSet());
            _player2.Verify(v => v.UseShuffledDiscardedSet());
        }

        [Test]
        public void Game_Play_PlayerWithHigherScoreShouldWin()
        {
            //Arrange
            var deck = new Mock<IDeck<Card>>();
            var cards = new List<Card> { new Card(1), new Card(2) };
            deck.Setup(v => v.Cards).Returns(new Queue<Card>(cards));
            deck.Setup(v => v.Count).Returns(2);

            var shuffler = new Mock<IShuffler<Card>>();
            var gamingInterface = new Mock<IGamingInterface>();
            _player1.Setup(v => v.DiscardedCardCount).Returns(1);
            _player1.Setup(v => v.ShowCard()).Returns(new Card(1));
            _player1.Setup(v => v.GetTotalAvailableCardsCount()).Returns(2);

            _player2.Setup(v => v.DiscardedCardCount).Returns(1);
            _player2.Setup(v => v.ShowCard()).Returns(new Card(2));
            _player2.Setup(v => v.GetTotalAvailableCardsCount()).Returns(2);
            IList<IPlayer<Card>> players = new List<IPlayer<Card>>() { _player1.Object, _player2.Object };
            _game = new Game<Card>(gamingInterface.Object, deck.Object, shuffler.Object, players, 2, 2, new System.Random());

            //Action
            _game.Play();

            //Assert
            _player2.Verify(v => v.AddDiscardedCard(It.Is<IList<Card>>(v => v.Count == 2)));
            gamingInterface.Verify(v => v.RoundCompleted(It.Is<string>(l => l == "2")));
        }

        [Test]
        public void Game_PlayWithSameScore_NextRoundWinnerShouldGet4Points()
        {
            //Arrange
            var deck = new Mock<IDeck<Card>>();
            var cards = new List<Card> { new Card(1), new Card(2) };
            deck.Setup(v => v.Cards).Returns(new Queue<Card>(cards));
            deck.Setup(v => v.Count).Returns(2);
            var shuffler = new Mock<IShuffler<Card>>();
            var gamingInterface = new Mock<IGamingInterface>();
            Queue<Card> firstQueue = new Queue<Card>(new List<Card>() { new Card(1), new Card(4) });
            _player1.Setup(v => v.ShowCard()).Returns(firstQueue.Dequeue);
            _player1.Setup(v => v.DiscardedCardCount).Returns(1);
            _player1.Setup(v => v.GetTotalAvailableCardsCount()).Returns(2);

            Queue<Card> SecondQueue = new Queue<Card>(new List<Card>() { new Card(1), new Card(2) });
            _player2.Setup(v => v.ShowCard()).Returns(SecondQueue.Dequeue);
            _player2.Setup(v => v.DiscardedCardCount).Returns(1);
            _player2.Setup(v => v.GetTotalAvailableCardsCount()).Returns(2);
            IList<IPlayer<Card>> players = new List<IPlayer<Card>>() { _player1.Object, _player2.Object };
            _game = new Game<Card>(gamingInterface.Object, deck.Object, shuffler.Object, players, 2, 2, new System.Random());

            //Action
            _game.Play();

            //Assert
            _player1.Verify(v => v.AddDiscardedCard(It.Is<IList<Card>>(v => v.Count == 4)));
        }

        [Test]
        public void Game_Initialize_EachPlayerShouldBeAssignedWithEqualCards()
        {
            //Arrange
            var deck = new Mock<IDeck<Card>>();
            var cards = new List<Card> { new Card(1), new Card(2) };
            deck.Setup(v => v.Cards).Returns(new Queue<Card>(cards));
            deck.Setup(v => v.Count).Returns(2);
            var shuffler = new Mock<IShuffler<Card>>();
            var gamingInterface = new Mock<IGamingInterface>();
            IList<IPlayer<Card>> players = new List<IPlayer<Card>>() { _player1.Object, _player2.Object };

            //Action
            _game = new Game<Card>(gamingInterface.Object, deck.Object, shuffler.Object, players, 2, 2, new System.Random());

            //Assert
            _player1.Verify(v => v.DeckCards.SetDeck(It.Is<IList<Card>>(v => v.Count == 1)));
            _player2.Verify(v => v.DeckCards.SetDeck(It.Is<IList<Card>>(v => v.Count == 1)));
        }

        #region input validation test

        [Test]
        public void Game_OnlyOnePlayer_ThrowsArgumentException()
        {
            //Arrange
            var deck = new Mock<IDeck<Card>>();
            var cards = new List<Card> { new Card(1), new Card(2) };
            deck.Setup(v => v.Cards).Returns(new Queue<Card>(cards));
            deck.Setup(v => v.Count).Returns(2);
            var shuffler = new Mock<IShuffler<Card>>();
            var gamingInterface = new Mock<IGamingInterface>();
            IList<IPlayer<Card>> players = new List<IPlayer<Card>>() { _player1.Object};


            //Action //Assert
            Assert.Throws<ArgumentException>(() => new Game<Card>(gamingInterface.Object, deck.Object, shuffler.Object, players, 2, 2, new Random()));
        }

        [Test]
        public void Game_NullDeck_ThrowsArgumentNullException()
        {
            //Arrange
            var cards = new List<Card> { new Card(1), new Card(2) };
            var shuffler = new Mock<IShuffler<Card>>();
            var gamingInterface = new Mock<IGamingInterface>();
            IList<IPlayer<Card>> players = new List<IPlayer<Card>>() { _player1.Object };

            //Action //Assert
            Assert.Throws<ArgumentNullException>(() => new Game<Card>(gamingInterface.Object, null, shuffler.Object, players, 2, 2, new Random()));
        }

        [Test]
        public void Game_NullGamingInterface_ThrowsArgumentNullException()
        {
            //Arrange
            var deck = new Mock<IDeck<Card>>();
            var cards = new List<Card> { new Card(1), new Card(2) };
            deck.Setup(v => v.Cards).Returns(new Queue<Card>(cards));
            deck.Setup(v => v.Count).Returns(2);
            var shuffler = new Mock<IShuffler<Card>>();
            IList<IPlayer<Card>> players = new List<IPlayer<Card>>() { _player1.Object };

            //Action //Assert
            Assert.Throws<ArgumentNullException>(() => new Game<Card>(null, deck.Object, shuffler.Object, players, 2, 2, new Random()));
        }

        [Test]
        public void Game_NullShuffler_ThrowsArgumentNullException()
        {
            //Arrange
            var deck = new Mock<IDeck<Card>>();
            var cards = new List<Card> { new Card(1), new Card(2) };
            deck.Setup(v => v.Cards).Returns(new Queue<Card>(cards));
            deck.Setup(v => v.Count).Returns(2);
            var gamingInterface = new Mock<IGamingInterface>();
            IList<IPlayer<Card>> players = new List<IPlayer<Card>>() { _player1.Object };

            //Action //Assert
            Assert.Throws<ArgumentNullException>(() => new Game<Card>(gamingInterface.Object, deck.Object, null, players, 2, 2, new Random()));
        }

        [Test]
        public void Game_NoPlayer_ThrowsArgumentException()
        {
            //Arrange
            var deck = new Mock<IDeck<Card>>();
            var cards = new List<Card> { new Card(1), new Card(2) };
            deck.Setup(v => v.Cards).Returns(new Queue<Card>(cards));
            deck.Setup(v => v.Count).Returns(2);
            var shuffler = new Mock<IShuffler<Card>>();
            var gamingInterface = new Mock<IGamingInterface>();
            IList<IPlayer<Card>> players = new List<IPlayer<Card>>();

            //Action //Assert
            Assert.Throws<ArgumentException>(() => new Game<Card>(gamingInterface.Object, deck.Object, shuffler.Object, players, 2, 2, new Random()));
        }

        [Test]
        public void Game_NullPlayer_ThrowsArgumentNullException()
        {
            //Arrange
            var deck = new Mock<IDeck<Card>>();
            var cards = new List<Card> { new Card(1), new Card(2) };
            deck.Setup(v => v.Cards).Returns(new Queue<Card>(cards));
            deck.Setup(v => v.Count).Returns(2);
            var shuffler = new Mock<IShuffler<Card>>();
            var gamingInterface = new Mock<IGamingInterface>();

            //Action //Assert
            Assert.Throws<ArgumentNullException>(() => new Game<Card>(gamingInterface.Object, deck.Object, shuffler.Object, null, 2, 2, new Random()));
        }

        [Test]
        public void Game_1CardAsTotalNumberOfCards_ThrowsArgumentException()
        {
            //Arrange
            var deck = new Mock<IDeck<Card>>();
            var cards = new List<Card> { new Card(1), new Card(2) };
            deck.Setup(v => v.Cards).Returns(new Queue<Card>(cards));
            deck.Setup(v => v.Count).Returns(2);
            var shuffler = new Mock<IShuffler<Card>>();
            var gamingInterface = new Mock<IGamingInterface>();
            IList<IPlayer<Card>> players = new List<IPlayer<Card>>();

            //Action //Assert
            Assert.Throws<ArgumentException>(() => new Game<Card>(gamingInterface.Object, deck.Object, shuffler.Object, players, 1, 2, new Random()));
        }

        [Test]
        public void Game_MaxCardValueAs1_ThrowsArgumentException()
        {
            //Arrange
            var deck = new Mock<IDeck<Card>>();
            var cards = new List<Card> { new Card(1), new Card(2) };
            deck.Setup(v => v.Cards).Returns(new Queue<Card>(cards));
            deck.Setup(v => v.Count).Returns(2);
            var shuffler = new Mock<IShuffler<Card>>();
            var gamingInterface = new Mock<IGamingInterface>();
            IList<IPlayer<Card>> players = new List<IPlayer<Card>>();

            //Action //Assert
            Assert.Throws<ArgumentException>(() => new Game<Card>(gamingInterface.Object, deck.Object, shuffler.Object, players, 2, 1, new Random()));
        }
        #endregion
    }
}
