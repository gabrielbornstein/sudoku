using System;
using GEB.Sudoku;
using Google.Cloud.Firestore;

namespace GEB
{
    class Program
    {
        static void Main(string[] args)
        {
            BoardPosition boardPosition = new BoardPosition
            {
                Row = 0,
                Column = 2
            };
            Sudoku.Sudoku.TestFunction(boardPosition, "1111").Wait();
            GameStatus statusSent = new GameStatus
            {
                GamePaused = true
            };
            Sudoku.Sudoku.UpdateCloudGameStatus(statusSent, "1111").Wait();

            FirestoreDb db = FirestoreDb.Create("sudoku-87c4a");
            DocumentReference reference = db.Collection("statuses").Document("1111");
            DocumentSnapshot snapshot = reference.GetSnapshotAsync().Result;
            GameStatus statusReturned = snapshot.ConvertTo<GameStatus>();
            Console.WriteLine(statusReturned);

            BoardMove move = new BoardMove
            {
                Row = 6,
                Column = 9,
                Value = 2
            };
            GameStatus gameStatus = new GameStatus
            {
                LastMove = move,

            };
            GameConfig config = new GameConfig
            {
                Difficulty = 1,
                Player1Id = "1111",

            };
            Sudoku.Sudoku.UpdateCloudGameStatus(gameStatus, "1111").Wait();
            //Sudoku.Sudoku.GetSudokuService().CreateNewGame(config);
            GameInstance game = Sudoku.Sudoku.GetSudokuService().GetGame("62b1e8b7-4f48-45cf-a663-bada78a02155");
            //Sudoku.Sudoku.GetSudokuService().RegisterPlayer("Kellyanne");
            //Player player = Sudoku.Sudoku.GetSudokuService().GetPlayer("45718b06-a9e3-4dd0-ab52-23aeaa2e0136");
            //Console.WriteLine(player.PlayerName);
            //Console.WriteLine(player.Score);

            GameInstance gameInstance = new GameInstance
            {
                Config = config,
                Status = gameStatus,
                GameId = "2222"
            };

        }
    }
}
