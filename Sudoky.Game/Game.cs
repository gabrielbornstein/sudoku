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
            for(int i=0;i< boardSize; i++)
            {
                for(int j=0;j< boardSize; j++)
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
            while  (k < 3500)            // (!BoardSolved())
            {
                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        if (board[i, j] == GridValueEnum.Blank)
                        {
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
                k++;
            }
        }

        public GridValueEnum GetGridValue(int row, int col)
        {
            GridValueEnum mask = GetBitMaskForRow(row);
            mask = mask | GetBitMaskForColumn(col);
            mask = mask | GetBitMaskForGrid(row / gridSize, col / gridSize);
            return GetValueForRowCol(mask);
        }

        private GridValueEnum GetBitMaskForRow(int row)
        {
            GridValueEnum mask = 0;
            for(int i=0;i<boardSize;i++)
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

            for(int i=0;i<boardSize+1;i++)
            {
                if(((int)currMask & (1 << i)) == 0)
                {
                    if (result != -1)
                        return GridValueEnum.Blank;

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

        public GridValueEnum GetValueForSquare(int row, int col)
        {
            GridValueEnum mask = GetPossibleValuesForRowCol(row, col);
            for (int i = 1; i < boardSize + 1; i++)
            {
                if (((int)mask & 1 << i) != 0)
                {
                    if (!IsValuePresentInOtherSquares(row / gridSize, col / gridSize, row, col, (GridValueEnum)(1 << i)))
                    {
                        return (GridValueEnum)(1 << i);
                    }
                }
            }
            return GridValueEnum.Blank;
        }

        private bool IsValuePresentInOtherSquares(int anchorRow, int anchorCol, int row, int col, GridValueEnum value)
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
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
                    }
                }
            }
            return false;
        }

        private bool IsBitSet(int value, int mask)
        {
            return ((value & mask) != 0);
        }

        private bool BoardSolved()
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
        }
        /*
        public GridValueEnum compareBitMaskToOtherSquares(int anchorRow, int anchorCol, GridValueEnum currentSpaceMask)
        {
            // Ignore test for Blank
            currentSpaceMask &= ~GridValueEnum.Blank;
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    int row = i + (anchorRow * gridSize);
                    int col = j + (anchorCol * gridSize);
                    if (board[row,col] == GridValueEnum.Blank)
                    {
                        GridValueEnum nextSpaceMask = getPossibleValuesForRowCol(row, col);
                        nextSpaceMask &= ~GridValueEnum.Blank;
                        for (int k = 1; k < boardSize + 1; k++)
                        {
                            if (IsBitSet((int)currentSpaceMask, 1 << k))
                            {
                                if(IsBitSet((int)nextSpaceMask, 1 << k))
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return currentSpaceMask;
        }
        */
    }
}
