using System;

namespace GEB.Sudoku
{
    public class Player : SudokuError
    {
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesFinished { get; set; }
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
