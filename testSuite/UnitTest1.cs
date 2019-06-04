using NUnit.Framework;
using GEB.Sudoku;
using System.Collections.Generic;

namespace Tests
{
    public class Testing
    {
        GEB.Sudoku.Sudoku myGame = null;
        GameInstance currGame = null;
        string currGameId = null;

        [SetUp]
        public void Setup()
        {
            myGame = new GEB.Sudoku.Sudoku();
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

            GEB.Sudoku.Sudoku game = new GEB.Sudoku.Sudoku(board1);
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

            GEB.Sudoku.Sudoku game = new GEB.Sudoku.Sudoku(board1);
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

            GEB.Sudoku.Sudoku game = new GEB.Sudoku.Sudoku(board1);
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

            GEB.Sudoku.Sudoku game = new GEB.Sudoku.Sudoku(board1);
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
            GEB.Sudoku.Sudoku myGame2 = new GEB.Sudoku.Sudoku();

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
            Assert.IsNotNull(myGame.GetGame(currGame.GameId));
            myGame.CancelGame(currGame.GameId);
            Assert.IsNull(myGame.GetGame(currGame.GameId));
        }

        [Test]
        public void TestVerifyBoard()
        {
            int[,] currBoard =
                {
                { 5, 3, 0, 0, 7, 0, 0, 0, 0 },
                { 6, 0, 0, 1, 9, 5, 0, 0, 0 },
                { 0, 9, 8, 0, 0, 0, 0, 6, 0 },
                { 8, 0, 0, 0, 6, 0, 0, 0, 3 },
                { 4, 0, 0, 8, 0, 3, 0, 0, 1 },
                { 7, 0, 0, 0, 2, 0, 0, 0, 6 },
                { 0, 6, 0, 0, 0, 0, 2, 8, 0 },
                { 0, 0, 0, 4, 1, 9, 0, 0, 5 },
                { 0, 0, 0, 0, 8, 0, 0, 7, 9 },
            };
            currGame.Config.InitBoard = currBoard;
            currGame.Status.CurrentBoard.Board = currBoard;
            int[,] completeBoard =
                {
                {5,3,4,6,7,8,9,1,2},
                {6,7,2,1,9,5,3,4,8},
                {1,9,8,3,4,2,5,6,7},
                {8,5,9,7,6,1,4,2,3},
                {4,2,6,8,5,3,7,9,1},
                {7,1,3,9,2,4,8,5,6},
                {9,6,1,5,3,7,2,8,4},
                {2,8,7,4,1,9,6,3,5},
                {3,4,5,2,8,6,1,7,9}
                };
            GameBoard solvedBoard = myGame.ShowFinishedBoard(currGame.GameId);
            Assert.AreEqual(solvedBoard.Board, completeBoard);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    BoardMove move = new BoardMove
                    {
                        Row = i,
                        Column = j,
                        Value = solvedBoard.Board[i, j]
                    };
                    myGame.SetBoardValue(currGame.GameId, move, false);
                }
            }
            Assert.AreEqual(currGame.Status.CurrentBoard.Board, completeBoard);

            int[,] badBoard =
                {
                {6,3,4,6,7,8,9,1,2},
                {6,7,2,1,9,5,3,4,8},
                {1,9,8,3,4,2,5,6,7},
                {8,5,9,7,6,1,4,2,3},
                {4,2,6,8,5,3,7,9,1},
                {7,1,3,9,2,4,8,5,6},
                {9,6,1,5,3,7,2,8,4},
                {2,8,7,4,1,9,6,3,5},
                {3,4,5,2,8,6,1,7,9}
                };

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    BoardMove move = new BoardMove
                    {
                        Row = i,
                        Column = j,
                        Value = badBoard[i,j]
                    };
                    myGame.SetBoardValue(currGame.GameId, move, false);
                }
            }

            Assert.AreNotEqual(currGame.Status.CurrentBoard, badBoard);

            BoardMove move1 = new BoardMove
            {
                Row = 0,
                Column = 2
            };
            int[,] currBoard1 =
    {
                { 5, 3, 0, 0, 7, 0, 0, 0, 0 },
                { 6, 0, 0, 1, 9, 5, 0, 0, 0 },
                { 0, 9, 8, 0, 0, 0, 0, 6, 0 },
                { 8, 0, 0, 0, 6, 0, 0, 0, 3 },
                { 4, 0, 0, 8, 0, 3, 0, 0, 1 },
                { 7, 0, 0, 0, 2, 0, 0, 0, 6 },
                { 0, 6, 0, 0, 0, 0, 2, 8, 0 },
                { 0, 0, 0, 4, 1, 9, 0, 0, 5 },
                { 0, 0, 0, 0, 8, 0, 0, 7, 9 },
            };
            currGame.Status.CurrentBoard.Board = currBoard1;
            List<int> correctList1 = new List<int>() {1, 2, 4 };
            List<int> testList1 = myGame.GetPossibleBoardValues(currGame.GameId, move1);
            Assert.AreEqual(correctList1, testList1);

        }

        /*
         * Verify board
         * 1: ShowFinishedBoard
         * 2: For each empty space in the current board, set the next open space based on the Completed board using SetBoardValue
         * 3: For each empty space verify the list of GetPossibleBoardValues (and make sure the value to be set is included in the list)
         * 4: GetGame and verify that the status is finished NOT SURE WHAT OR HOW (should be attached to check move?)
         * Should also test:
         * GetPossibleBoardMove
         * GetCurrentBoardStatus
         * More thorough testing in general
         */

    }
}