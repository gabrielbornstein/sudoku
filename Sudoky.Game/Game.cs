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

    public class Game //: IGame
    {
        const int gridSize = 3;
        const int boardSize = gridSize * gridSize;
        const int numGivensEasy = 51;
        const int numGivensRegular = 56;
        const int numGivensHard = 61;
        public GridValueEnum[,] board { get; set; } = null;

        public delegate void NotifyBoardUpdated(Game sender, BoardUpdatedEventArgs evtArgs);
        public event NotifyBoardUpdated NotifyBoardUpdatedEvent;

        public Game()
        {
            board = new GridValueEnum[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    board[i, j] = GridValueEnum.Blank;
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
                                        value = board[i, j]
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
            for (int i = 0; i < boardSize; i++)
            {
                mask |= board[row, i];
            }
            return mask;
        }

        private GridValueEnum GetBitMaskForColumn(int col)
        {
            GridValueEnum mask = 0;
            for (int i = 0; i < boardSize; i++)
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
            return (result != -1) ? (GridValueEnum)result : GridValueEnum.Blank;
        }

        public GridValueEnum GetPossibleValuesForRowCol(int row, int col)
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

        public static Dictionary<string, Player> IDPlayerDict = new Dictionary<string, Player>();

        public GameResult DeletePlayer(string playerId)
        {
            Player pl = GetPlayer(playerId);
            if (pl != null)
            {
                IDPlayerDict.Remove(playerId);
            }

            return new GameResult()
            {
                Error = (pl != null) ? GameErrorEnum.OK : GameErrorEnum.InvalidPlayerID,
                Result = (pl != null)
            };
        }

        public GameResult RenamePlayer(string playerId, string playerName)
        {
            Player pl = GetPlayer(playerId);
            if (pl != null)
            {
                pl.PlayerName = playerName;
            }

            return new GameResult()
            {
                Error = (pl != null) ? GameErrorEnum.OK : GameErrorEnum.InvalidPlayerID,
                Result = (pl != null)
            };
        }

        public Player GetPlayer(string playerId)
        {
            Player tmpPlayer;
            return (IDPlayerDict.TryGetValue(playerId, out tmpPlayer)) ? tmpPlayer : null;
        }

        public Player RegisterPlayer(string name)
        {
            Player player = new Player()
            {
                PlayerName = name,
                GamesPlayed = 0,
                GamesFinished = 0,
                Score = 0,
                Error = GameErrorEnum.OK
            };

            //generate user ID
            player.PlayerId = Guid.NewGuid().ToString();
            IDPlayerDict.Add(player.PlayerId, player);

            return player;
        }

        public GameStatus CreateNewGame(GameConfig config, int difficulty)
        {
            GameStatus game = new GameStatus
            {
                GameId = Guid.NewGuid().ToString(),
                Board = MakeBoard(difficulty),
                NextPlayerId = config.Player2Id,
                LastMove = new BoardMove(),
                GamePaused = false,
                Error = GameErrorEnum.OK
            };
           
            return game;
        }

        public GridValueEnum[,] MakeBoard(int difficulty)
        {
                FillBoard();
                DeleteSpaces(difficulty);
                return board;               
        }

        public GridValueEnum[,] FillBoard()
        {
            Random rnd = new Random();
            for (int i = 0; i < boardSize; i++)
            {
                List<GridValueEnum> possibleValues = new List<GridValueEnum>();
                GridValueEnum possibleValuesMask = GetPossibleValuesForRowCol(0, i);
                for (int k = 1; k < boardSize + 1; k++)
                {
                    if (((int)possibleValuesMask & (1 << k)) != 0)
                    {
                        possibleValues.Add((GridValueEnum)(1 << k));
                    }
                }
                    int r = rnd.Next(possibleValues.Count);
                    board[0, i] = possibleValues[r];
                    possibleValues.Clear(); 
            }
            ShiftBoard(3, 1);
            ShiftBoard(3, 2);
            ShiftBoard(1, 3);
            ShiftBoard(3, 4);
            ShiftBoard(3, 5);
            ShiftBoard(1, 6);
            ShiftBoard(3, 7);
            ShiftBoard(3, 8);
            return board;
        }

        private void ShiftBoard(int ShiftSize, int row)
        {
            for (int i = 0; i < boardSize; i++)
            {
                if (i + ShiftSize >= boardSize)
                    board[row, i] = board[row - 1, (i + ShiftSize) % boardSize];
                else
                    board[row, i] = board[row - 1, i + ShiftSize];
            }
        }

        public GridValueEnum[,] DeleteSpaces(int difficulty)
        {
            if (difficulty == 1)
            {
                for (int numDeletions = 0; numDeletions < numGivensEasy; numDeletions++)
                {
                    DeleteSpace();
                }
            }

            if (difficulty == 2)
            {
                for (int numDeletions = 0; numDeletions < numGivensRegular; numDeletions++)
                {
                    DeleteSpace();
                }
            }

            if (difficulty == 3)
            {
                for (int numDeletions = 0; numDeletions < numGivensHard; numDeletions++)
                {
                    DeleteSpace();
                }
            }
            return board;
        }

        private void DeleteSpace()
        {

            GridValueEnum[,] copyBoard = (GridValueEnum[,])board.Clone();
            int numFails = 0;

            do
            {
                PickRandomSpace(out int row, out int col);
                GridValueEnum originalSpace = board[row, col];
                board[row, col] = GridValueEnum.Blank;

                SolveEntireBoard();

                if (BoardIsSolved())
                {
                    numFails = 0;
                    copyBoard[row, col] = GridValueEnum.Blank;
                    board = (GridValueEnum[,])copyBoard.Clone();
                    return;
                }
                else
                {
                    numFails++;
                    board[row, col] = originalSpace;
                    if (numFails > boardSize)
                    {
                        return;
                    }
                }

            } while (!BoardIsSolved());
            return;
        }

        private void PickRandomSpace(out int row, out int col)
        {
            Random random = new Random();
            do
            {
                row = random.Next(0, boardSize - 1);
                col = random.Next(0, boardSize - 1);
            } while (board[row, col] == GridValueEnum.Blank);
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

        #endregion
    }
}

