﻿using System;
using GEB.Sudoku;
using Tests;

namespace GEB
{
    class Program
    {
        static void Main(string[] args)
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

            Game currGame = new Game(board1);
            currGame.SolveEntireBoard();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.WriteLine(currGame.board[i,j]);
                }
            }
        }
    }
}
