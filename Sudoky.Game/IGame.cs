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
        GameErrorEnum Error { get; set; }
    }

    public class Player: GameError
    {
        string PlayerId { get; set; }
        string PlayerName { get; set; }
        int gamesPlayed { get; set; }
        int gamesFinished { get; set; }
        int score { get; set; }
    }

    public class BoardPosition : GameError
    {
        int Row { get; set; }
        int Column { get; set; }
    }

    public class BoardMove : BoardPosition
    {
        int Value { get; set; }
        String PlayerId { get; set; }
    }

    public class GameConfig : GameError
    {
        int GridSize { get; set; }          // Supports 3x3, 4x4, etc
        int[,] InitBoard { get; set; }      // Optional
        String Player1Id { get; set; }
        String Player2Id { get; set; }      // Optional
        int MaxTimePerMove { get; set; }    // Optional - defaults to 0 or no timeout
        bool EnableAssistMode { get; set; } // Optional - defaults to false
    }

    public class GameStatus : GameError
    {
        String GameId { get; set; }
        int[,] Board { get; set; }
        String NextPlayerId { get; set; }
        BoardMove LastMove { get; set; }
        bool GamePaused { get; set; }
    }

    public class GameResult : GameError
    {
        bool Result { get; set; }
    }

    public interface IGame
    {
        // Player management
        Player RegisterPlayer(string playerName);
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
