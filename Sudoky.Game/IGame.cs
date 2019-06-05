using System;
using System.Collections.Generic;

namespace GEB.Sudoku
{
    [Flags]
    public enum GridValueEnum
    {
        Blank = 1 << 0,
        Digit_1 = 1 << 1,
        Digit_2 = 1 << 2,
        Digit_3 = 1 << 3,
        Digit_4 = 1 << 4,
        Digit_5 = 1 << 5,
        Digit_6 = 1 << 6,
        Digit_7 = 1 << 7,
        Digit_8 = 1 << 8,
        Digit_9 = 1 << 9
    }

    public class BoardPosition : SudokuError
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }

    public class PossibleBoardValues : SudokuError
    {
        public List<int> PossibleValues { get; set; }
    }

    public class BoardMove : BoardPosition
    {
        public int Value { get; set; }
        public String PlayerId { get; set; }
    }

    public class GameConfig : SudokuError
    {
        public int[,] InitBoard { get; set; }      // Optional
        public int[,] CompletedBoard { get; set; }
        public String Player1Id { get; set; }
        public String Player2Id { get; set; }      // Optional
        public int MaxTimePerMove { get; set; }    // Optional - defaults to 0 or no timeout
        public bool EnableAssistMode { get; set; } // Optional - defaults to false
        public int Difficulty { get; set; }
    }

    public class GameBoard : SudokuError
    {
        public int[,] Board { get; set; }
    }

    public class GameStatus
    {
        public GameBoard CurrentBoard { get; set; }
        public String NextPlayerId { get; set; }
        public BoardMove LastMove { get; set; }
        public bool GamePaused { get; set; }
        public bool GameCompleted { get; set; }
    }

    public class GameInstance
    {
        public GameConfig Config { get; set; }
        public GameStatus Status { get; set; }
        public String GameId { get; set; }
    }

    public class GameResult : SudokuError
    {
        public bool Result { get; set; }
    }

    public interface IGame
    {
        // Create, Delete or Pause/Start Game
        GameInstance CreateNewGame(GameConfig config);
        GameResult CancelGame(string gameId);
        GameResult PauseGame(string gameId, bool pause);

        GameStatus GetCurrentBoardStatus(string gameId);
        GameBoard ShowFinishedBoard(string gameId);

        GameResult SetBoardValue(string gameId, BoardMove value);
        PossibleBoardValues GetPossibleBoardValues(string id, BoardMove pos);
        BoardMove GetPossibleBoardMove(string id);
    }
}
