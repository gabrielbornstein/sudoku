using NUnit.Framework;
using GEB.Sudoku;

namespace Tests
{
    public class Testing
    {
        Game myGame = null;
        GameInstance currGame = null;
        string currGameId = null;

        [SetUp]
        public void Setup()
        {
            myGame = new Game();
            Assert.IsNotNull(myGame);

            Player player1 = myGame.RegisterPlayer("Dan The Man");
            Assert.IsNotNull(player1);

            currGame = myGame.CreateNewGame(new GameConfig()
            {
                Player1Id = player1.PlayerId,
                Difficulty = 1
            });
            Assert.IsNotNull(currGame);

            currGameId = currGame.GameId;
            Assert.IsNotNull(currGameId);
        }


        [Test]
        public void Test1()
        {
            GridValueEnum[,] board1 = new GridValueEnum[,]
            {
                { GridValueEnum.Digit_5, GridValueEnum.Digit_3, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_7, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank },
                { GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_1, GridValueEnum.Digit_9, GridValueEnum.Digit_5, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank },
                { GridValueEnum.Blank, GridValueEnum.Digit_9, GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank },
                { GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_3 },
                { GridValueEnum.Digit_4, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Digit_3, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_1 },
                { GridValueEnum.Digit_7, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_2, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6 },
                { GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_2, GridValueEnum.Digit_8, GridValueEnum.Blank },
                { GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_4, GridValueEnum.Digit_1, GridValueEnum.Digit_9, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_5 },
                { GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_8, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Digit_7, GridValueEnum.Digit_9 },
            };

            Game game = new Game(board1);
            GridValueEnum value = game.GetGridValue(6, 8);
            Assert.AreEqual(value, GridValueEnum.Digit_4);
        }

        [Test]
        public void Test2()
        {
            GridValueEnum[,] board1 = new GridValueEnum[,]
            {
                { GridValueEnum.Digit_5, GridValueEnum.Digit_3, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_7, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank },
                { GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_1, GridValueEnum.Digit_9, GridValueEnum.Digit_5, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank },
                { GridValueEnum.Blank, GridValueEnum.Digit_9, GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank },
                { GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_3 },
                { GridValueEnum.Digit_4, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Digit_3, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_1 },
                { GridValueEnum.Digit_7, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_2, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6 },
                { GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_2, GridValueEnum.Digit_8, GridValueEnum.Blank },
                { GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_4, GridValueEnum.Digit_1, GridValueEnum.Digit_9, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_5 },
                { GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_8, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Digit_7, GridValueEnum.Digit_9 },
            };

            Game game = new Game(board1);
            GridValueEnum value = game.GetGridValue(6, 8);
            Assert.AreEqual(GridValueEnum.Digit_4, value);
        }
        [Test]
        public void Test3()
        {
            GridValueEnum[,] board1 = new GridValueEnum[,]
             {
                { GridValueEnum.Digit_5, GridValueEnum.Digit_3, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_7, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank },
                { GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_1, GridValueEnum.Digit_9, GridValueEnum.Digit_5, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank },
                { GridValueEnum.Blank, GridValueEnum.Digit_9, GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank },
                { GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_3 },
                { GridValueEnum.Digit_4, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Digit_3, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_1 },
                { GridValueEnum.Digit_7, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_2, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6 },
                { GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_2, GridValueEnum.Digit_8, GridValueEnum.Blank },
                { GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_4, GridValueEnum.Digit_1, GridValueEnum.Digit_9, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_5 },
                { GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_8, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Digit_7, GridValueEnum.Digit_9 },
            };

            Game game = new Game(board1);
            GridValueEnum value = game.GetValueForSquare(0, 5);
            Assert.AreEqual(GridValueEnum.Digit_8, value);
        }
        [Test]
        public void Test4()
        {
            GridValueEnum[,] board1 = new GridValueEnum[,]
             {
                { GridValueEnum.Digit_5, GridValueEnum.Digit_3, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_7, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank },
                { GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_1, GridValueEnum.Digit_9, GridValueEnum.Digit_5, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank },
                { GridValueEnum.Blank, GridValueEnum.Digit_9, GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank },
                { GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_3 },
                { GridValueEnum.Digit_4, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Digit_3, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_1 },
                { GridValueEnum.Digit_7, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_2, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6 },
                { GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_2, GridValueEnum.Digit_8, GridValueEnum.Blank },
                { GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_4, GridValueEnum.Digit_1, GridValueEnum.Digit_9, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_5 },
                { GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_8, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Digit_7, GridValueEnum.Digit_9 },
            };

            Game game = new Game(board1);
            GridValueEnum value = game.GetValueForSquare(5, 2);
            Assert.AreEqual(GridValueEnum.Digit_3, value);
        }

        // Player Testing
        #region Player Testing

        [Test]
        public void TestPlayer()
        {
            Player dk = myGame.RegisterPlayer("David Krinker");
            Player gb = myGame.RegisterPlayer("Gabriel Bornstein");

            // Now make sure we can query player
            Player dk1 = myGame.GetPlayer(dk.PlayerId);
            Assert.AreEqual(dk, dk1);

            Player gb1 = myGame.GetPlayer(gb.PlayerId);
            Assert.AreEqual(gb, gb1);

            Assert.AreNotEqual(dk, gb);

            // Try new Game
            Game myGame2 = new Game();

            dk1 = myGame2.GetPlayer(dk.PlayerId);
            Assert.AreEqual(dk, dk1);

            GameResult result = myGame2.DeletePlayer(dk.PlayerId);
            Assert.IsTrue(result.Result);

            dk1 = myGame2.GetPlayer(dk.PlayerId);
            Assert.IsNull(dk1);

            Player player1 = myGame.RegisterPlayer("Dan The Man");
            myGame.RenamePlayer(player1.PlayerId, "Not Dan");
            Assert.AreEqual(player1.PlayerName, "Not Dan");
        }
        #endregion

        [Test]
        public void TestPauseGame()
        {
            // Make sure we can pause the game
            GameResult result = myGame.PauseGame(currGame.GameId, true);
            Assert.IsTrue(result.Result);

            currGame = myGame.GetGame(currGame.GameId);
            Assert.IsNotNull(currGame);
            Assert.IsTrue(currGame.Status.GamePaused);
        }

        [Test]
        public void TestUnPauseGame()
        {
            // Make sure we can pause the game
            GameResult result = myGame.PauseGame(currGame.GameId, false);
            Assert.IsTrue(result.Result);

            currGame = myGame.GetGame(currGame.GameId);
            Assert.IsNotNull(currGame);
            Assert.IsFalse(currGame.Status.GamePaused);
        }

        [Test]
        public void TestCancelGame()
        {
            myGame.CancelGame(currGame.GameId);
            Assert.IsNull(myGame.GetGame(currGame.GameId));
        }
        /*
         * Cancel Game Test
         * 1: Cancel a game
         * 2: GetGame
         * 3: verify GetGame comes back as null
         */

        /*
         * Verify board
         * 1: GetCompletedBoard
         * 2: For each empty space in the current board, set the next open space based on the Completed board using SetBoardValue
         * 3: For each empty space verify the list of GetPossibleBoardValues (and make sure the value to be set is included in the list)
         * 4: GetGame and verify that the status is finished
         */

    }
}