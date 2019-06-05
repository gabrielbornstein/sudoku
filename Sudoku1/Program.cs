using System;
using GEB.Sudoku;

namespace GEB
{
    class Program
    {
        static void Main(string[] args)
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

            int[,] solvedBoard = GEB.Sudoku.Sudoku.GetSudokuService().SolveEntireBoard(board1);
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    System.Console.Write(solvedBoard[row, col]);
                }
                System.Console.WriteLine();
            }
            Console.WriteLine();
            /*
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
                { GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_7, GridValueEnum.Digit_9 },
            };

            Game currGame = new Game(board1);
            currGame.NotifyBoardUpdatedEvent += NotifyBoardUpdated;
            currGame.SolveEntireBoard();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(currGame.board[i,j] + " ");
                }
                Console.WriteLine();
            }

            Game game = new Game();
            int[,] board = { 
                           {0,0,0,0,0,0,0,0,0 },
                           { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                           {0,0,0,0,0,0,0,0,0 },
                           {0,0,0,0,0,0,0,0,0 },
                           {0,0,0,0,0,0,0,0,0 },
                           {0,0,0,0,0,0,0,0,0 },
                           {0,0,0,0,0,0,0,0,0 },
                           {0,0,0,0,0,0,0,0,0 },
                           {0,0,0,0,0,0,0,0,0 }
                           };
            board = game.FillBoard();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(board[i,j]);
                }
                Console.WriteLine();
            }
        }

        public static void NotifyBoardUpdated(Game sender, BoardUpdatedEventArgs evtArgs)
        {
            Console.WriteLine("Set Board: Row {0}, Column {1}, Value = {2}", evtArgs.row, evtArgs.col, evtArgs.value);
        }
        */
            int[,] board = GEB.Sudoku.Sudoku.GetSudokuService().MakeBoard(1);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(board[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            int[,] SolvedBoard = GEB.Sudoku.Sudoku.GetSudokuService().SolveEntireBoard(board);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(SolvedBoard[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
