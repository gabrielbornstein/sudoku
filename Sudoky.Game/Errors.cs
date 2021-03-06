﻿using System;
using Google.Cloud.Firestore;

namespace GEB.Sudoku
{
    public enum SudokuErrorEnum
    {
        OK = 0,
        InvalidGameID,
        InvalidMove,
        InvalidPlayerID,
        Timeout,
        OutOfTurn,
        InternalError,
        InitialBoardValueCannotChange
    }

    public class SudokuError
    {
        public SudokuErrorEnum Error { get; set; }

        public String ErrorDescription { get;set;}

        public SudokuError() { }

        public SudokuError(SudokuErrorEnum error, String description)
        {
            Error = error;
            ErrorDescription = description; 
        }
    }
}