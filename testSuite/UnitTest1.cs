using NUnit.Framework;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using GEB.Sudoku;

namespace Tests
{
    public class Testing
    {
        // GEB.Sudoku.Sudoku myGame = null;
        //GameInstance currGame = null;
        //string currGameId = null;

        [SetUp]
        public void Setup()
        {
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "/users/gezrab/Projects/sudoku/Sudoku-2075c08d560b.json");
            GameInstance myGame = Sudoku.GetSudokuService().CreateNewGame(new GameConfig() {
                Difficulty = 1,
                Player1Id = "1234" });
            Assert.IsNotNull(myGame);

            Player player1 = Sudoku.GetSudokuService().RegisterPlayer("Dan The Man");
            Assert.IsNotNull(player1);

            GameInstance currGame = Sudoku.GetSudokuService().CreateNewGame(new GameConfig()
            {
                Player1Id = player1.PlayerId,
                Difficulty = 1
            });
            Assert.IsNotNull(currGame);

            string currGameId = currGame.GameId;
            Assert.IsNotNull(currGameId);
        }

        /*
        [Test]
        public void Test1()
        {
            int[,] board1 =
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

            Sudoku game = new Sudoku();
            int value = game.GetValueForSquare(board1, 6, 8);
            Assert.AreEqual(value, GridValueEnum.Digit_4);
        }
        */
        [Test]
        public void Test3()
        {
            int[,] board1 =
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
            int value = Sudoku.GetSudokuService().GetValueForSquare(board1, 0, 5);
            Assert.AreEqual(8, value);
        }
        /*
        [Test]
        public void Test4()
        {
            int[,] board1 =
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
            Sudoku game = new Sudoku();
            int value = game.GetValueForSquare(board1, 6, 8);
            Assert.AreEqual(4, value);
        }
        */
        [Test]
        public void Test5()
        {
            int[,] board1 =
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
            int[,] solvedBoard = Sudoku.GetSudokuService().SolveEntireBoard(board1);
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    System.Console.Write(solvedBoard[row, col]);
                }
                System.Console.WriteLine();
            }
        }

        // Player Testing
        #region Player Testing

        [Test]
        public void TestPlayer()
        {

            Player dk = Sudoku.GetSudokuService().RegisterPlayer("David Krinker");
            Player gb = Sudoku.GetSudokuService().RegisterPlayer("Gabriel Bornstein");

            // Now make sure we can query player
            Player dk1 = Sudoku.GetSudokuService().GetPlayer(dk.PlayerId);
            Assert.AreEqual(dk, dk1);

            Player gb1 = Sudoku.GetSudokuService().GetPlayer(gb.PlayerId);
            Assert.AreEqual(gb, gb1);

            Assert.AreNotEqual(dk, gb);

            // Try new Game

            dk1 = Sudoku.GetSudokuService().GetPlayer(dk.PlayerId);
            Assert.AreEqual(dk, dk1);

            GameResult result = Sudoku.GetSudokuService().DeletePlayer(dk.PlayerId);
            Assert.IsTrue(result.Result);

            dk1 = Sudoku.GetSudokuService().GetPlayer(dk.PlayerId);
            Assert.IsNull(dk1);

            Player player1 = Sudoku.GetSudokuService().RegisterPlayer("Dan The Man");
            Sudoku.GetSudokuService().RenamePlayer(player1.PlayerId, "Not Dan");
            Assert.AreEqual(player1.PlayerName, "Not Dan");
        }
        #endregion

        [Test]
        public void TestPauseGame()
        {
            GameInstance currGame = Sudoku.GetSudokuService().CreateNewGame(new GameConfig {
                Difficulty = 1,
                Player1Id = "1234" });
            // Make sure we can pause the game
            GameResult result = Sudoku.GetSudokuService().PauseGame(currGame.GameId, true);
            Assert.IsTrue(result.Result);

            currGame = Sudoku.GetSudokuService().GetGame(currGame.GameId);
            Assert.IsNotNull(currGame);
            Assert.IsTrue(currGame.Status.GamePaused);
        }

        [Test]
        public void TestUnPauseGame()
        {
            GameInstance currGame = Sudoku.GetSudokuService().CreateNewGame(new GameConfig
            {
                Difficulty = 1,
                Player1Id = "1234"
            });
            // Make sure we can pause the game
            GameResult result = Sudoku.GetSudokuService().PauseGame(currGame.GameId, false);
            Assert.IsTrue(result.Result);

            currGame = Sudoku.GetSudokuService().GetGame(currGame.GameId);
            Assert.IsNotNull(currGame);
            Assert.IsFalse(currGame.Status.GamePaused);
        }

        [Test]
        public void TestCancelGame()
        {
            GameInstance currGame = Sudoku.GetSudokuService().CreateNewGame(new GameConfig
            {
                Difficulty = (int)DifficultyLevel.Easy,
            });

            Assert.IsNotNull(Sudoku.GetSudokuService().GetGame(currGame.GameId));
            Sudoku.GetSudokuService().CancelGame(currGame.GameId);
            Assert.IsNull(Sudoku.GetSudokuService().GetGame(currGame.GameId));
        }

        [Test]
        public void TestVerifyBoard()
        {
            GameInstance currGame = Sudoku.GetSudokuService().CreateNewGame(new GameConfig
            {
                Difficulty = 1,
                Player1Id = "1234"
            });
            Assert.AreEqual(currGame.Config.InitBoard, currGame.Status.CurrentBoard.Board);
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
                        { 0, 0, 0, 0, 8, 0, 0, 7, 9 }
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
            GameBoard solvedBoard = Sudoku.GetSudokuService().ShowFinishedBoard(currGame.GameId);
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
                    Sudoku.GetSudokuService().SetBoardValue(currGame.GameId, move);
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
                        Value = badBoard[i, j]
                    };
                    Sudoku.GetSudokuService().SetBoardValue(currGame.GameId, move);
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
            List<int> correctList1 = new List<int>() { 1, 2, 4 };
            List<int> testList1 = Sudoku.GetSudokuService().GetPossibleBoardValues(currGame.GameId, move1).PossibleValues;
            Assert.AreEqual(correctList1, testList1);

            }
        [Test]
        public void TestUpdateCloudGameStatus()
        {
            GameStatus statusSent = new GameStatus
            {
                GamePaused = false
            };
            Sudoku.UpdateCloudGameStatus(statusSent, "1111").Wait();

            FirestoreDb db = FirestoreDb.Create("sudoku-87c4a");
            DocumentReference reference = db.Collection("statuses").Document("1111");
            DocumentSnapshot snapshot = reference.GetSnapshotAsync().Result;
            GameStatus statusReturned = snapshot.ConvertTo<GameStatus>();
            Assert.AreEqual(statusSent, statusReturned);
        }
    }            
}
