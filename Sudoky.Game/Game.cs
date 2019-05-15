using System;

namespace GEB.Sudoku
{
    public class Game
    {
        const int gridSize = 3;
        const int boardSize = gridSize * gridSize;
        public GridValueEnum[,] board { get; set; } = null;

        public Game()
        {
            board = new GridValueEnum[boardSize,boardSize];
            for (int i = 0; i < boardSize; i++)
            {
                for(int j = 0; j < boardSize; j++)
                {
                    board[i,j] = GridValueEnum.Blank;
                }
            }
        }

        public Game(GridValueEnum[,] initBoard)
        {
            board = initBoard;
        }

        public void SolveEntireBoard()
        {
            int k = 0;
            int numBlanks = -1;
            while (!BoardIsSolved())
            {
                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        if (board[i, j] == GridValueEnum.Blank)
                        {
                            k++;
                            GridValueEnum value = GetGridValue(i, j);
                            if (value == GridValueEnum.Blank)
                            {
                                board[i, j] = GetValueForSquare(i, j);
                            }
                            else
                            {
                                board[i, j] = value;
                            }
                            if (board[i, j] != GridValueEnum.Blank)
                            {
                                Console.WriteLine("Set Board: Row {0}, Column {1}, Value = {2}", i, j, board[i, j]);
                            }
                        }
                    }
                }
                if (k == numBlanks)
                {
                    Console.WriteLine("Problem: Board cannot be solved with current algorithm");
                    break;
                }
                numBlanks = k;
                k = 0;
            }
        }

        public GridValueEnum GetGridValue(int row, int col)
        {
            GridValueEnum mask = GetBitMaskForRow(row);
            mask = mask | GetBitMaskForColumn(col);
            mask = mask | GetBitMaskForGrid(row / gridSize, col / gridSize);
<<<<<<< HEAD
            return GetValueForRowCol(mask);
=======
            board[row,col] = GetValueForRowCol(mask);
            return board[row, col];
        }

        public GridValueEnum GetValueForSquare(int row, int col)
        {
            GridValueEnum mask = GetPossibleValuesForRowCol(row, col);
            for (int i = 1; i < boardSize + 1; i++)
            {
                if (((int)mask & 1 << i) != 0)
                {
                    if (!IsValuePresentInOtherSquares(row, col, (GridValueEnum)(1 << i)))
                    {
                        return (GridValueEnum)(1 << i);
                    }
                }
            }
            return GridValueEnum.Blank;
        }

        private bool BoardIsSolved()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (board[i, j] == GridValueEnum.Blank)
                        return false;
                }
            }
            return true;
>>>>>>> 6b29b45... * Program.cs: Cleaned up solution
        }

        private GridValueEnum GetBitMaskForRow(int row)
        {
            GridValueEnum mask = 0;
            for(int i = 0; i < boardSize; i++)
            {
                mask |= board[row, i];
            }
            return mask;
        }

        private GridValueEnum GetBitMaskForColumn(int col)
        {
            GridValueEnum mask = 0;
            for(int i = 0; i < boardSize; i++)
            {
                mask |= board[i, col];
            }
            return mask;
        }

        private GridValueEnum GetBitMaskForGrid(int anchorRow, int anchorCol)
        {
            GridValueEnum mask = 0;
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    {
                        mask |= board[(anchorRow * gridSize) + i, (anchorCol * gridSize) + j];
                    }
                }
            }
            return mask;
        }

        private GridValueEnum GetValueForRowCol(GridValueEnum currMask)
        {
            int result = -1;
            for (int i = 0; i < boardSize + 1; i++)
            {
                if (((int)currMask & (1 << i)) == 0)
                {
                    if (result != -1)
                    {
                        return GridValueEnum.Blank;
                    }
                    result = 1 << i;
                }
            }
            return (result != -1) ? (GridValueEnum) result : GridValueEnum.Blank;
        }

        private GridValueEnum GetPossibleValuesForRowCol(int row, int col)
        {
            GridValueEnum mask = GetBitMaskForRow(row);
            mask = mask | GetBitMaskForColumn(col);
            mask = mask | GetBitMaskForGrid(row / gridSize, col / gridSize);
            mask = ~(mask) & (GridValueEnum)0x3ff;
            return mask;
        }

        private bool IsValuePresentInOtherSquares(int row, int col, GridValueEnum value)
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
<<<<<<< HEAD
                    int currRow = i + (anchorRow * gridSize);
                    int currCol = j + (anchorCol * gridSize);
                    if (!(currRow == row && currCol == col))
                    {
                        if (board[currRow, currCol] == GridValueEnum.Blank)
                        {
                            GridValueEnum mask = GetPossibleValuesForRowCol(currRow, currCol);
                            if (IsBitSet((int)value, (int)mask))
                            {
                                return true;
                            }
                        }
=======
                    int currRow = i + ((row / gridSize) * gridSize);
                    int currCol = j + ((col / gridSize) * gridSize);
                    if (!(currRow == row && currCol == col))
                    {
                        if (board[currRow, currCol] == GridValueEnum.Blank)
                        {
                            GridValueEnum mask = GetPossibleValuesForRowCol(currRow, currCol);
                            if (IsBitSet((int)value, (int)mask))
                            {
                                return true;
                            }
                        }
>>>>>>> 6b29b45... * Program.cs: Cleaned up solution
                    }
                }
            }
            return false;
        }

        private bool IsBitSet(int value, int mask)
        {
            return ((value & mask) != 0);
        }

    }
}
