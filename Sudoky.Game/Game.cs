using System;

namespace GEB.Sudoku
{
    public class Game
    {
        const int gridSize = 3;
        const int boardSize = gridSize * gridSize;
        GridValueEnum[,] board = null;

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

        public GridValueEnum GetGridValue(int row, int col)
        {
            GridValueEnum mask = GetBitMaskForRow(row);
            mask = mask | GetBitMaskForColumn(col);
            mask = mask | GetBitMaskForGrid(row / gridSize, col / gridSize);
            board[row,col] = GetValueForRowCol(mask);
            return board[row, col];
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
                    if (board[(anchorRow * gridSize) + i, (anchorCol * gridSize) + j] != GridValueEnum.Blank)
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

        private GridValueEnum getPossibleValuesForRowCol(int row, int col)
        {
            GridValueEnum mask = GetBitMaskForRow(row);
            mask = mask | GetBitMaskForColumn(col);
            mask = mask | GetBitMaskForGrid(row / gridSize, col / gridSize);
            return mask;
        
        }

        public GridValueEnum GetValueForSquare(int row, int col)
        {
            GridValueEnum mask = getPossibleValuesForRowCol(row, col);
        }

        private bool IsValuePresentInOtherSquares(int anchorRow, int anchorCol, GridValueEnum value)
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    int row = i + (anchorRow * gridSize);
                    int col = j + (anchorCol * gridSize);
                    if (board[row, col] == GridValueEnum.Blank)
                    {
                        GridValueEnum mask = getPossibleValuesForRowCol(row, col);
                        if(IsBitSet((int)value, (int)mask))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

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

        private bool IsBitSet(int value, int mask)
        {
            return ((value & mask) != 0);
        }

        public bool isBoardSolved()
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
    }
}
