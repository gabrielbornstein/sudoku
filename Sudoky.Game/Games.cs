using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Google.Cloud.Firestore;
using System.Threading.Tasks;

namespace GEB.Sudoku
{
    public enum DifficultyLevel
    {
        Easy = 1,
        Medium = 2,
        Hard = 3
    }

    public partial class Sudoku : IGame
    {
        const int gridSize = 3;
        const int boardSize = gridSize * gridSize;
        const int numGivensEasy = 51;
        const int numGivensRegular = 56;
        const int numGivensHard = 61;

        private static Object lockObj = new Object();
        private static Sudoku svc = null;

        public static Dictionary<string, GameInstance> gamesRepo = new Dictionary<string, GameInstance>();

        private Sudoku()
        {
            // Initialize Sudoku Service here
        }

        public static Sudoku GetSudokuService()
        {
            lock (lockObj)
            {
                if (svc == null)
                {
                    svc = new Sudoku();
                }
            }
            return svc;
        }

        #region Interface Implementation

        public GameInstance GetGame(string gameId)
        {
            GameInstance tmpGameInstance;
            gamesRepo.TryGetValue(gameId, out tmpGameInstance);
            if (tmpGameInstance == null)
            {
                tmpGameInstance = GetGameFromCloud(gameId).Wait();
                if (tmpGameInstance == null)
                {
                    return null;
                }
            }
            return tmpGameInstance;
            //return (gamesRepo.TryGetValue(gameId, out tmpGameInstance)) ? tmpGameInstance : null;
        }

        public GameInstance CreateNewGame(GameConfig config)
        {
            config.CompletedBoard = CreateCompletedBoard();
            config.InitBoard = DeleteSpaces(config.CompletedBoard, (DifficultyLevel)config.Difficulty);

            GameInstance game = new GameInstance
            {
                Status = new GameStatus
                {
                    CurrentBoard = new GameBoard
                    {
                        Board = CloneBoard(config.InitBoard)
                    },
                    NextPlayerId = config.Player1Id,
                    LastMove = null,
                    GamePaused = false,
                    GameCompleted = false,
                },
                Config = config,
                GameId = Guid.NewGuid().ToString()
            };
            gamesRepo.TryAdd(game.GameId, game);
            UpdateCloudGameInstance(game, game.GameId).Wait();
            return game;
        }

        public GameResult CancelGame(string gameId)
        {
            GameInstance instance = GetGame(gameId);
            if (instance != null)
            {
                gamesRepo.Remove(gameId);
                DeleteCloudGame(gameId).Wait();
                return new GameResult
                {
                    Result = true,
                    Error = SudokuErrorEnum.OK
                };
            }
            else
            {
                return new GameResult
                {
                    Result = false,
                    Error = SudokuErrorEnum.InvalidGameID
                };
            }
        }

        public GameResult PauseGame(string gameId, bool pause)
        {
            GameInstance instance = GetGame(gameId);
            if (instance != null)
            {
                instance.Status.GamePaused = pause;
                UpdateCloudGameStatus(instance.Status, gameId).Wait();
                return new GameResult
                {
                    Result = true,
                    Error = SudokuErrorEnum.OK
                };
            } 
            else
            {
                return new GameResult
                {
                    Result = false,
                    Error = SudokuErrorEnum.InvalidGameID
                };
            }
        }

        public GameList GamesByPlayer(string playerId)
        {
            List<String> list = new List<string>();
            foreach (var game in gamesRepo.Values)
            {
                if (game.Config.Player1Id == playerId ||
                   (game.Config.Player2Id != null && game.Config.Player2Id == playerId))
                {
                    list.Add(game.GameId);
                }
            }

            return new GameList()
            {
                Error = SudokuErrorEnum.OK,
                Games = list
            };
        }

        public GameStatus GetCurrentBoardStatus(string gameId)
        {
            GameInstance instance = GetGame(gameId);
            if (instance != null)
            {
                return instance.Status;
            }
            else
            {
                return new GameStatus
                {
                    CurrentBoard = new GameBoard
                    {
                        Error = SudokuErrorEnum.InvalidGameID
                    }
                };
            }
        }

        public GameBoard ShowFinishedBoard(string gameId)
        {
            GameInstance instance = GetGame(gameId);
            if (instance != null)
            {
                if (instance.Config.CompletedBoard != null)
                {
                    return new GameBoard
                    {
                        Board = instance.Config.CompletedBoard,
                        Error = SudokuErrorEnum.OK
                    };
                } else
                {
                    return new GameBoard
                    {
                        ErrorDescription = "",
                        Error = SudokuErrorEnum.InternalError
                    };
                }
            }
            else
            {
                //make sure a copy of the board is filled, not actual board
                return new GameBoard
                {
                    Error = SudokuErrorEnum.InvalidGameID,
                    ErrorDescription = string.Format("No game found with gameID {0}", gameId)
                };
            }
        }

        public GameResult SetBoardValue(string gameId, BoardMove move)
        {
            GameInstance instance = GetGame(gameId);
            if (instance != null)
            {
                if (instance.Config.InitBoard[move.Row, move.Column] == 0)
                {
                    bool isValueValid = true;
                    // maybe check for the correct value
                    if(instance.Config.EnableAssistMode)
                    {
                        List<int> possibleValues = GetPossibleValuesForRowCol(instance.Status.CurrentBoard.Board, move.Row, move.Column);
                        isValueValid = possibleValues.Contains(move.Value);
                    }
                    if (isValueValid)
                    {
                        instance.Status.CurrentBoard.Board[move.Row, move.Column] = move.Value;
                        instance.Status.LastMove = move;
                        //check if board is finished
                        if (instance.Status.CurrentBoard.Board == ShowFinishedBoard(gameId).Board)
                        {
                            instance.Status.GameCompleted = BoardsAreEqual(instance.Status.CurrentBoard.Board, instance.Config.CompletedBoard);
                        }
                        UpdateCloudGameStatus(instance.Status, gameId).Wait();
                        return new GameResult
                        {
                            Result = true,
                            Error = SudokuErrorEnum.OK
                        };
                    } 
                    else
                    {
                        return new GameResult
                        {
                            Result = false,
                            Error = SudokuErrorEnum.InvalidMove,
                            ErrorDescription = "Value is not a valid value"
                        };
                    }
                }
                else
                {
                    return new GameResult
                    {
                        Result = false,
                        Error = SudokuErrorEnum.InitialBoardValueCannotChange
                    };
                }
            }
            else
            {
                //make sure a copy of the board is filled, not actual board
                return new GameResult
                {
                    Error = SudokuErrorEnum.InvalidGameID,
                    ErrorDescription = string.Format("No game found with gameID {0}", gameId)
                };
            }
        }

        public PossibleBoardValues GetPossibleBoardValues(string gameId, BoardMove pos)
        {
            GameInstance instance = GetGame(gameId);
            if (instance != null)
            {
                return new PossibleBoardValues
                {
                    Error = SudokuErrorEnum.OK,
                    PossibleValues = GetPossibleValuesForRowCol(instance.Status.CurrentBoard.Board, pos.Row, pos.Column)
                };
            }
            else
            {
                //make sure a copy of the board is filled, not actual board
                return new PossibleBoardValues
                {
                    Error = SudokuErrorEnum.InvalidGameID,
                    ErrorDescription = string.Format("No game found with gameID {0}", gameId)
                };
            }
        }

        public BoardMove GetPossibleBoardMove(string gameId)
        {
            GameInstance instance = GetGame(gameId);
            if (instance != null)
            {
                var rc = PickRandomBlankSpace(instance.Status.CurrentBoard.Board);
                return new BoardMove
                {
                    Row = rc.row,
                    Column = rc.col,
                    Value = instance.Config.CompletedBoard[rc.row,rc.col],
                    Error = SudokuErrorEnum.OK,
                };
            }
            else
            {
                //make sure a copy of the board is filled, not actual board
                return new BoardMove
                {
                    Error = SudokuErrorEnum.InvalidGameID,
                    ErrorDescription = string.Format("No game found with gameID {0}", gameId)
                };
            }
        }

        #endregion

        private static FirestoreDb InitializeDataBase()
        {
            FirestoreDb db = FirestoreDb.Create("sudoku-87c4a");
            return db;
        }

        private static async Task UpdateCloudGameStatus(GameStatus status, string gameId)
        {
            FirestoreDb db = InitializeDataBase();
            DocumentReference reference = db.Collection("games").Document(gameId).Collection("statuses").Document("status");
            await reference.SetAsync(status);
        }

        private static async Task UpdateCloudGameInstance(GameInstance instance, string gameId)
        {
            FirestoreDb db = InitializeDataBase();
            DocumentReference reference = db.Collection("games").Document(gameId);
            await reference.SetAsync(instance);
        }

        private static async Task<GameInstance> GetGameFromCloud(string gameId)
        {
            FirestoreDb db = InitializeDataBase();
            DocumentReference reference = db.Collection("games").Document(gameId);
            DocumentSnapshot snapshot = await reference.GetSnapshotAsync();
            GameInstance game = snapshot.ConvertTo<GameInstance>();
            return game;
        }

        private static async Task DeleteCloudGame(string gameId)
        {
            FirestoreDb db = InitializeDataBase();
            DocumentReference reference = db.Collection("games").Document(gameId);
            await reference.DeleteAsync();
        }

        private bool BoardsAreEqual(int[,] board1, int[,] board2)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (board1[i, j] != board2[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private int[,] CreateEmptyBoard()
        {
            int[,] board = new int[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    board[i, j] = 0;
                }
            }

            return board;
        }

        private int[,] CloneBoard(int[,] board)
        {
            int[,] clone = new int[boardSize, boardSize];
            for(int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    clone[i, j] = board[i, j];
                }
            }
            return clone;
        }

        public int[,] SolveEntireBoard(int[,] board)
        {
            int[,] completedBoard = CloneBoard(board);

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
                        if (completedBoard[i, j] == 0)
                        {
                            currNumBlanks++;
                            int value = GetValueForRowCol(GetPossibleValuesForRowCol(completedBoard, i ,j));
                            if (value == 0)
                            {
                                completedBoard[i, j] = GetValueForSquare(completedBoard, i, j);
                            }
                            else
                            {
                                completedBoard[i, j] = value;
                            }
                        }
                    }
                }
            } while (!BoardIsSolved(completedBoard) && currNumBlanks != prevNumBlanks);

            return (BoardIsSolved(completedBoard)) ? completedBoard : null;
        }

        public int GetValueForSquare(int[,] board, int row, int col)
        {
            List<int> possibleValues = GetPossibleValuesForRowCol(board, row, col);
            for (int i = 0; i < possibleValues.Count(); i++)
            {
                if (!IsValuePresentInOtherSquares(board, row, col, possibleValues[i]))
                {
                    return possibleValues[i];
                }
            }
            return 0;
        }

        private bool BoardIsSolved(int[,] board)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (board[i, j] == 0)
                        return false;
                }
            }
            return true;
        }

        private List<int> GetBitMaskForRow(int[,] board, ref List<int> list, int row)
        {
            for (int col = 0; col < boardSize; col++)
            {
                if (board[row, col] != 0)
                {
                    list.Remove(board[row, col]);
                }
            }

            return list;          
        }

        private List<int> GetBitMaskForColumn(int[,] board, ref List<int> list, int col)
        {
            for (int row = 0; row < boardSize; row++)
            {
                if (board[row, col] != 0)
                {
                    list.Remove(board[row, col]);
                }
            }
            return list;
        }

        private List<int> GetBitMaskForGrid(int[,] board, ref List<int> list, int anchorRow, int anchorCol)
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    {
                        if (board[(anchorRow * gridSize) + i, (anchorCol * gridSize) + j] != 0)
                        {
                            list.Remove(board[(anchorRow * gridSize) + i, (anchorCol * gridSize) + j]);
                        }
                    }
                }
            }
            return list;
        }

        private int GetValueForRowCol(List<int> possibleValues)
        {
            if (possibleValues.Count == 1)
            {
                return possibleValues[0];
            }
            else
            {
                return 0;
            }          
        }

        public List<int> GetPossibleValuesForRowCol(int[,] board, int row, int col)
        {
            List<int> possibleValues = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9 };
            GetBitMaskForRow(board, ref possibleValues, row);
            GetBitMaskForColumn(board, ref possibleValues, col);
            GetBitMaskForGrid(board, ref possibleValues, row / gridSize, col / gridSize);
            return possibleValues;          
        }

        private bool IsValuePresentInOtherSquares(int[,] board, int row, int col, int value)
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    int currRow = i + ((row / gridSize) * gridSize);
                    int currCol = j + ((col / gridSize) * gridSize);
                    if (!(currRow == row && currCol == col))
                    {
                        if (board[currRow, currCol] == 0)
                        {
                            List<int> possibleValues = GetPossibleValuesForRowCol(board, currRow, currCol);
                            if (possibleValues.Contains(value))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public int[,] CreateCompletedBoard()
        {
            int[,] board = new int[boardSize, boardSize];
            Random rnd = new Random();
            for (int i = 0; i < boardSize; i++)
            {
                List<int> possibleValues = GetPossibleValuesForRowCol(board, 0, i);

                int r = rnd.Next(possibleValues.Count);
                board[0, i] = possibleValues[r];
                possibleValues.Clear();
            }
            ShiftBoard(board, 3, 1);
            ShiftBoard(board, 3, 2);
            ShiftBoard(board, 1, 3);
            ShiftBoard(board, 3, 4);
            ShiftBoard(board, 3, 5);
            ShiftBoard(board, 1, 6);
            ShiftBoard(board, 3, 7);
            ShiftBoard(board, 3, 8);
            return board;
        }

        private void ShiftBoard(int[,] board, int ShiftSize, int row)
        {
            for (int i = 0; i < boardSize; i++)
            {
                if (i + ShiftSize >= boardSize)
                    board[row, i] = board[row - 1, (i + ShiftSize) % boardSize];
                else
                    board[row, i] = board[row - 1, i + ShiftSize];
            }
        }

        public int[,] DeleteSpaces(int[,] board, DifficultyLevel difficulty)
        {
            int totalSpacesToDelete;

            switch(difficulty)
            {
                case  DifficultyLevel.Easy:
                    totalSpacesToDelete = numGivensEasy;
                    break;
                case DifficultyLevel.Medium:
                    totalSpacesToDelete = numGivensRegular;
                    break;
                case DifficultyLevel.Hard:
                    totalSpacesToDelete = numGivensHard;
                    break;
                default:
                    throw new Exception("Unexpected difficulty board level");
            }

            return RemoveSpaces(board, totalSpacesToDelete);
        }

        private List<BoardPosition> GetRandomBoardPositionList()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            List<BoardPosition> boardPositions = new List<BoardPosition>();
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    int index = random.Next(0, boardPositions.Count);
                    boardPositions.Insert(index, new BoardPosition
                    {
                        Row = i,
                        Column = j
                    });
                }
            }
            return boardPositions;
        }

        private int[,] RemoveSpaces(int[,] board, int totalSpacesToDelete)
        {
            List<BoardPosition> randomPositions = GetRandomBoardPositionList();

            int[,] copyBoard = CloneBoard(board);
            for(int index = 0; index < randomPositions.Count && totalSpacesToDelete > 0; index++)
            {
                BoardPosition boardPos = randomPositions[index];

                int originalValue = copyBoard[boardPos.Row, boardPos.Column];
                copyBoard[boardPos.Row, boardPos.Column] = 0;

                if( SolveEntireBoard(copyBoard) != null) { 
                    totalSpacesToDelete--;
                }
                else
                {
                    copyBoard[boardPos.Row, boardPos.Column] = originalValue;
                }
            }
            return copyBoard;
        }

        private (int row, int col) PickRandomSpace(int[,] board)
        {
            int r, c;

            Random random = new Random();
            do
            {
                r = random.Next(0, boardSize);
                c = random.Next(0, boardSize);
            } while (board[r, c] == 0);

            return (r, c);
        }

        private (int row, int col) PickRandomBlankSpace(int[,] board)
        {
            int r, c;

            Random random = new Random();
            do
            {
                r = random.Next(0, boardSize);
                c = random.Next(0, boardSize);
            } while (board[r, c] != 0);

            return (r,c);
        }
    }
}

