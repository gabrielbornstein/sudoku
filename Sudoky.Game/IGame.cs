﻿using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
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

    [FirestoreData]
    public class BoardPosition : SudokuError
    {
        [FirestoreProperty]
        public int Row { get; set; }
        [FirestoreProperty]
        public int Column { get; set; }
    }

    public class PossibleBoardValues : SudokuError
    {
        public List<int> PossibleValues { get; set; }
    }

    public class GameList : SudokuError
    {
        public List<String> Games { get; set; }
    }

    [FirestoreData]
    public class BoardMove : BoardPosition
    {
        [FirestoreProperty]
        public int Value { get; set; }
        [FirestoreProperty]
        public String PlayerId { get; set; }
    }

    public static class BoardExtensions
    {
        public static List<int> ConvertBoardToList(this int[,] board)
        {
            List<int> l = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    l.Add(board[i, j]);
                }
            }
            return l;
        }

        public static int[,] ConvertListToBoard(this List<int> list)
        {
            int[,] board = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    board[i, j] = list[j + i * 9];
                }
            }
            return board;
        }
    }

    [FirestoreData]
    public class GameConfig : SudokuError
    {
        public int[,] InitBoard { get; set; }
        [FirestoreProperty]
        public List<int> InitBoard2
        {
            get
            {
                return InitBoard.ConvertBoardToList();
            }
            set
            {
                InitBoard = value.ConvertListToBoard();
            }
        }

        public int[,] CompletedBoard { get; set; }
        [FirestoreProperty]
        public List<int> CompletedBoard2
        {
            get
            {
                return CompletedBoard.ConvertBoardToList();
            }
            set
            {
                CompletedBoard = value.ConvertListToBoard();
            }
        }

        [FirestoreProperty]
        public String Player1Id { get; set; }
        [FirestoreProperty]
        public String Player2Id { get; set; }      // Optional
        [FirestoreProperty]
        public bool EnableAssistMode { get; set; } // Optional - defaults to false
        [FirestoreProperty]
        public int Difficulty { get; set; }
    }

    [FirestoreData]
    public class GameBoard : SudokuError
    {
        public int[,] Board { get; set; }
        [FirestoreProperty]
        public List<int> Board2
        {
            get
            {
                return Board.ConvertBoardToList();
            }
            set
            {
                Board = value.ConvertListToBoard();
            }
        }
    }

    [FirestoreData]
    public class GameStatus
    {
        [FirestoreProperty]
        public GameBoard CurrentBoard { get; set; }
        [FirestoreProperty]
        public String NextPlayerId { get; set; }
        [FirestoreProperty]
        public BoardMove LastMove { get; set; }
        [FirestoreProperty]
        public bool GamePaused { get; set; }
        [FirestoreProperty]
        public bool GameCompleted { get; set; }
    }

    [FirestoreData]
    public class GameInstance
    {
        [FirestoreProperty]
        public GameConfig Config { get; set; }
        [FirestoreProperty]
        public GameStatus Status { get; set; }
        [FirestoreProperty]
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

        GameList GamesByPlayer(string playerId);

        GameStatus GetCurrentBoardStatus(string gameId);
        GameBoard ShowFinishedBoard(string gameId);

        GameResult SetBoardValue(string gameId, BoardMove value);
        PossibleBoardValues GetPossibleBoardValues(string id, BoardMove pos);
        BoardMove GetPossibleBoardMove(string id);
    }
}
