using System;
using Google.Cloud.Firestore;

namespace GEB.Sudoku
{
    [FirestoreData]
    public class Player : SudokuError
    {
        [FirestoreProperty]
        public string PlayerId { get; set; }
        [FirestoreProperty]
        public string PlayerName { get; set; }
        [FirestoreProperty]
        public int GamesPlayed { get; set; }
        [FirestoreProperty]
        public int GamesFinished { get; set; }
        [FirestoreProperty]
        public int Score { get; set; }
    }

    public interface IPlayers
    {
        // Player management
        Player RegisterPlayer(string playerName);
        Player GetPlayer(string playerId);
        GameResult DeletePlayer(string playerId);
        GameResult RenamePlayer(string playerId, string playerName);
    }
}
