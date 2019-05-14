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
            return IsGridValueKnown(mask);
        }

        private GridValueEnum GetBitMaskForRow(int row)
        {
            GridValueEnum mask = 0;
            for(int i=0;i<boardSize;i++)
            {
                if (board[row, i] != GridValueEnum.Blank)
                {
                    mask |= board[row, i];
                }
            }
            return mask;
        }

        private GridValueEnum GetBitMaskForColumn(int col)
        {
            GridValueEnum mask = 0;
            for(int i = 0; i < boardSize; i++)
            {
                if (board[i, col] != GridValueEnum.Blank)
                {
                    mask |= board[i, col];
                }
            }
            return mask;
        }

        private GridValueEnum GetBitMaskForGrid(int anchorRow, int anchorCol)
        {
            GridValueEnum mask = 0;
            for (int i = anchorRow; i < anchorRow + gridSize; i++)
            {
                for (int j = anchorCol; j < anchorCol + gridSize; j++)
                {
                    if (board[i, j] != GridValueEnum.Blank)
                    {
                        mask |= board[i, j];
                    }
                }
            }
            return mask;
        }

        private GridValueEnum IsGridValueKnown(GridValueEnum currMask)
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
