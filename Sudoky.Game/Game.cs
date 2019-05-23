using System;
using System.Collections.Generic;

namespace GEB.Sudoku
{
    public class BoardUpdatedEventArgs : EventArgs
    {
        public int row { get; set; }
        public int col { get; set; }
        public GridValueEnum value { get; set; }
    }

    public class Game : IGame
    {
        const int gridSize = 3;
        const int boardSize = gridSize * gridSize;
        public GridValueEnum[,] board { get; set; } = null;

        public delegate void NotifyBoardUpdated(Game sender, BoardUpdatedEventArgs evtArgs);
        public event NotifyBoardUpdated NotifyBoardUpdatedEvent;

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
            int currNumBlanks = 0;
            int prevNumBlanks = 0;
            do
            {
                prevNumBlanks = currNumBlanks;
                currNumBlanks = 0;

                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        if (board[i, j] == GridValueEnum.Blank)
                        {
                            currNumBlanks++;
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
                                // Notify that the board changed
                                if (NotifyBoardUpdatedEvent != null)
                                {
                                    NotifyBoardUpdatedEvent(this, new BoardUpdatedEventArgs()
                                    {
                                        row = i,
                                        col = j,
                                        value = board[i,j]
                                    });
                                }
                            }
                        }
                    }
                }
            } while (!BoardIsSolved() && currNumBlanks != prevNumBlanks);
            Console.WriteLine("Board {0}", (BoardIsSolved()) ? "Solved" : "Not Solved");
        }

        public GridValueEnum GetGridValue(int row, int col)
        {
            GridValueEnum mask = GetBitMaskForRow(row);
            mask = mask | GetBitMaskForColumn(col);
            mask = mask | GetBitMaskForGrid(row / gridSize, col / gridSize);
            return GetValueForRowCol(mask);
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
                    }
                }
            }
            return false;
        }

        private bool IsBitSet(int value, int mask)
        {
            return ((value & mask) != 0);
        }

#region Interface Implementation

        public Player RegisterPlayer(string playerName)
        {
            throw new NotImplementedException();
        }

        public GameResult DeletePlayer(string playerId)
        {
            throw new NotImplementedException();
        }

        public GameResult RenamePlayer(string playerId, string playerName)
        {
            throw new NotImplementedException();
        }

        public GameStatus CreateNewGame(GameConfig config)
        {
            throw new NotImplementedException();
        }

        public GameResult CancelGame(string gameId)
        {
            throw new NotImplementedException();
        }

        public GameResult PauseGame(string gameId, bool pause)
        {
            throw new NotImplementedException();
        }

        public GameStatus GetCurrentBoardStatus(string gameId)
        {
            throw new NotImplementedException();
        }

        public GameStatus GetCompletedBoard(string gameId)
        {
            throw new NotImplementedException();
        }

        public GameResult SetBoardValue(string gameId, BoardMove value, bool checkValue)
        {
            throw new NotImplementedException();
        }

        public List<int> GetPossibleBoardValues(string id, BoardMove pos)
        {
            throw new NotImplementedException();
        }

        public BoardMove GetPossibleBoardMove(string id)
        {
            throw new NotImplementedException();
        }
    }

#endregion

}
