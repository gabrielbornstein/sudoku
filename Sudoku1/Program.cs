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
                Column = 1
            };
            Sudoku.Sudoku.TestFunction(boardPosition, "1111").Wait();
        }
    }
}
