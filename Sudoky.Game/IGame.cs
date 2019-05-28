using System;
using System.Collections.Generic;

namespace GEB.Sudoku
{
    public enum GameErrorEnum
    {
        OK = 0,
        InvalidGameID,
        InvalidMove,
        InvalidPlayerID,
        Timeout,
        OutOfTurn
    }

    public class GameError
    {
        public GameErrorEnum Error { get; set; }
    }

    public class Player: GameError
    {
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesFinished { get; set; }
        public int Score { get; set; }
    }

    public class BoardPosition : GameError
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }

    public class BoardMove : BoardPosition
    {
        public int Value { get; set; }
        public String PlayerId { get; set; }
    }

    public class GameConfig : GameError
    {
        public int[,] InitBoard { get; set; }      // Optional
        public String Player1Id { get; set; }
        public String Player2Id { get; set; }      // Optional
        public int MaxTimePerMove { get; set; }    // Optional - defaults to 0 or no timeout
        public bool EnableAssistMode { get; set; } // Optional - defaults to false
    }

    public class GameStatus : GameError
    {
        public String GameId { get; set; }
        public GridValueEnum[,] Board { get; set; }
        public String NextPlayerId { get; set; }
        public BoardMove LastMove { get; set; }
        public bool GamePaused { get; set; }
    }

    //make a class that combines config and status
    //implement CreateNewGame (level of difficulty?)
    //do research on how a sudoku board is made

    public class GameResult : GameError
    {
        public bool Result { get; set; }
    }

    public interface IGame
    {
        // Player management
        Player RegisterPlayer(string playerName);
        Player GetPlayer(string playerId);
        GameResult DeletePlayer(string playerId);
        GameResult RenamePlayer(string playerId, string playerName);

        // Create, Delete or Pause/Start Game
        GameStatus CreateNewGame(GameConfig config);
        GameResult CancelGame(string gameId);
        GameResult PauseGame(string gameId, bool pause);

        GameStatus GetCurrentBoardStatus(string gameId);
        GameStatus GetCompletedBoard(string gameId);

        GameResult SetBoardValue(string gameId, BoardMove value, bool checkValue);
        List<int> GetPossibleBoardValues(string id, BoardMove pos);
        BoardMove GetPossibleBoardMove(string id);
    }
}
