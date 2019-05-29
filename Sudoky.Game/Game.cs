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

        public GridValueEnum[,] SolveEntireBoard(GridValueEnum[,] board)
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
            return board;
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

        private int CastGridValToInt(GridValueEnum gridValueEnum)
        {
            if (gridValueEnum == GridValueEnum.Blank)
            {
                return 0;
            }
            else if (gridValueEnum == GridValueEnum.Digit_1)
            {
                return 1;
            }
            else if (gridValueEnum == GridValueEnum.Digit_2)
            {
                return 2;
            }
            else if (gridValueEnum == GridValueEnum.Digit_3)
            {
                return 3;
            }
            else if (gridValueEnum == GridValueEnum.Digit_4)
            {
                return 4;
            }
            else if (gridValueEnum == GridValueEnum.Digit_5)
            {
                return 5;
            }
            else if (gridValueEnum == GridValueEnum.Digit_6)
            {
                return 6;
            }
            else if (gridValueEnum == GridValueEnum.Digit_7)
            {
                return 7;
            }
            else if (gridValueEnum == GridValueEnum.Digit_8)
            {
                return 8;
            }
            else if (gridValueEnum == GridValueEnum.Digit_9)
            {
                return 9;
            }
            else
            {
                Console.WriteLine("Error: Cannot caste this value");
                return -1;
            }
        }

        private GridValueEnum CastIntToGridValue(int x)
        {
            if (x == 0)
            {
                return GridValueEnum.Blank;
            }
            else if (x == 1)
            {
                return GridValueEnum.Digit_1;
            }
            else if (x == 2)
            {
                return GridValueEnum.Digit_2;
            }
            else if (x == 3)
            {
                return GridValueEnum.Digit_3;
            }
            else if (x == 4)
            {
                return GridValueEnum.Digit_4;
            }
            else if (x == 5)
            {
                return GridValueEnum.Digit_5;
            }
            else if (x == 6)
            {
                return GridValueEnum.Digit_6;
            }
            else if (x == 7)
            {
                return GridValueEnum.Digit_7;
            }
            else if (x == 8)
            {
                return GridValueEnum.Digit_8;
            }
            else if (x == 9)
            {
                return GridValueEnum.Digit_9;
            }
            else
            {
                Console.WriteLine("Error: Cannot caste this value");
                return GridValueEnum.Blank;
            }
        }

        #region Interface Implementation

        public static Dictionary<string, Player> IDPlayerDict = new Dictionary<string, Player>();
        public static Dictionary<string, GameInstance> IDGameDict = new Dictionary<string, GameInstance>();

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

        public GameInstance GetGame(string gameId)
        {
            GameInstance tmpGameInstance;
            return (IDGameDict.TryGetValue(gameId, out tmpGameInstance)) ? tmpGameInstance : null;
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
            IDPlayerDict.TryAdd(player.PlayerId, player);

            return player;
        }

        public GameInstance CreateNewGame(GameConfig config)
        {
            GameInstance game = new GameInstance
            {
                Status = new GameStatus
                {
                    Board = MakeBoard(config.Difficulty),
                    NextPlayerId = config.Player2Id,
                    LastMove = new BoardMove(),
                    GamePaused = false,
                    Error = GameErrorEnum.OK
                },
                Config = config,
                GameId = Guid.NewGuid().ToString()
            };
            IDGameDict.TryAdd(game.GameId, game);
            return game;  //game.Status Should this return an instance?
        }

        public int[,] MakeBoard(int difficulty)
        {
            FillBoard();
            DeleteSpaces(difficulty);
            int[,] intBoard = new int[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    intBoard[i, j] = CastGridValToInt(board[i, j]);
                }
            }
            return intBoard;               
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
                    copyBoard[row,col] = originalSpace;
                    board = (GridValueEnum[,])copyBoard.Clone();
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
                row = random.Next(0, boardSize);
                col = random.Next(0, boardSize);
            } while (board[row, col] == GridValueEnum.Blank);
        }

        private void PickRandomBlankSpace(out int row, out int col)
        {
            Random random = new Random();
            do
            {
                row = random.Next(0, boardSize);
                col = random.Next(0, boardSize);
            } while (board[row, col] != GridValueEnum.Blank);
        }

        public GameResult CancelGame(string gameId)
        {
            if (IDGameDict.ContainsKey(gameId))
            {
                IDGameDict.Remove(gameId);
                return new GameResult
                {
                    Result = true,
                    Error = GameErrorEnum.OK
                };
            }
            else
            {
                return new GameResult
                {
                    Result = false,
                    Error = GameErrorEnum.InvalidGameID
                };
            }
        }

        public GameResult PauseGame(string gameId, bool pause)
        {
            GetGame(gameId).Status.GamePaused = pause;
            GameResult result = new GameResult
            {
                Result = true,
                Error = GameErrorEnum.OK
            };
            return result;
        }

        public GameStatus GetCurrentBoardStatus(string gameId)
        {
            if (IDGameDict.ContainsKey(gameId))
            {
                GameInstance game = GetGame(gameId);
                game.Status.Error = GameErrorEnum.OK;
                return game.Status;
            }
            else
            {
                GameStatus status = new GameStatus
                {
                    Error = GameErrorEnum.InvalidGameID
                };
                return status;
            }
        }

        public GameStatus GetCompletedBoard(string gameId)
        {
            if (IDGameDict.ContainsKey(gameId))
            {
                GameInstance instance = GetGame(gameId);

                GridValueEnum[,] gridValueBoard = new GridValueEnum[boardSize, boardSize];
                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        gridValueBoard[i, j] = CastIntToGridValue(instance.Config.InitBoard[i, j]);
                    }
                }

                gridValueBoard = SolveEntireBoard(gridValueBoard);

                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        instance.Status.Board[i, j] = CastGridValToInt(gridValueBoard[i, j]);
                    }
                }
                return instance.Status;
            }
            else
            {
                return new GameStatus
                {
                    Error = GameErrorEnum.InvalidGameID
                };
            }
        }

        public GameResult SetBoardValue(string gameId, BoardMove value, bool checkValue)
        {
            //is the player allowed to make mistakes?
            if (GetGame(gameId).Config.InitBoard[value.Row, value.Column] == 0)
            {
                GetGame(gameId).Status.Board[value.Row, value.Column] = value.Value;
                return new GameResult
                {
                    Result = true,
                    Error = GameErrorEnum.OK
                };
            }
            else
            {
                return new GameResult
                {
                    Result = false,
                    Error = GameErrorEnum.InvalidMove
                };
            }
        }

        public List<int> GetPossibleBoardValues(string gameId, BoardMove pos)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    board[i, j] = CastIntToGridValue(GetGame(gameId).Status.Board[i, j]);
                }
            }

            GridValueEnum mask = GetPossibleValuesForRowCol(pos.Row, pos.Column);
            for (int i = 0; i < boardSize + 1; i++)
            {
                if (((int)mask & (1 << i)) != 0)
                {
                    list.Add(CastGridValToInt((GridValueEnum)(1 << i)));
                }
            }
            return list;
        }

        public BoardMove GetPossibleBoardMove(string gameId)
        {
            int value = 0;
            int numTries = 0;
            int currRow = -1, currCol = -1;

            //set board equal to specific game board
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    board[i, j] = CastIntToGridValue(GetGame(gameId).Status.Board[i, j]);
                }
            }

            do
            {
                PickRandomBlankSpace(out int row, out int col);
                currRow = row;
                currCol = col;
                value = CastGridValToInt(GetGridValue(row, col));

                if (value == 0)
                {
                    value = CastGridValToInt(GetValueForSquare(row, col));
                }

                if (numTries > numGivensEasy)
                {
                    //This Should Be An Announcement
                    Console.WriteLine("Cannot Find A Solution!");
                    return new BoardMove
                    {
                        Value = value,
                        Row = -1,
                        Column = -1,
                        Error = GameErrorEnum.Timeout
                    };
                }

                numTries++;
            } while (value == 0);

            BoardMove move = new BoardMove
            {
                Value = value,
                Row = currRow,
                Column = currCol,
            };
            return move;
        }

        #endregion
    }
}

