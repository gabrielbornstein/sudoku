using System;
using System.Linq;
using System.Collections.Generic;

namespace GEB.Sudoku
{
    public class BoardUpdatedEventArgs : EventArgs
    {
        public int row { get; set; }
        public int col { get; set; }
        public GridValueEnum value { get; set; }
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
            return (gamesRepo.TryGetValue(gameId, out tmpGameInstance)) ? tmpGameInstance : null;
        }

        public GameInstance CreateNewGame(GameConfig config)
        {
            int[,] board = CreateEmptyBoard();

            GameInstance game = new GameInstance
            {
                Status = new GameStatus
                {
                    CurrentBoard = new GameBoard
                    {
                        Board = MakeBoard(config.Difficulty)
                    },
                    NextPlayerId = config.Player2Id,
                    LastMove = new BoardMove(),
                    GamePaused = false,
                },
                Config = config,
                GameId = Guid.NewGuid().ToString()
            };
            game.Config.InitBoard = CloneBoard((int[,])game.Status.CurrentBoard.Board);
            gamesRepo.TryAdd(game.GameId, game);
            return game;
        }

        public GameResult CancelGame(string gameId)
        {
            if (gamesRepo.ContainsKey(gameId))
            {
                gamesRepo.Remove(gameId);
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
            GetGame(gameId).Status.GamePaused = pause;
            GameResult result = new GameResult
            {
                Result = true,
                Error = SudokuErrorEnum.OK
            };
            return result;
        }

        public GameStatus GetCurrentBoardStatus(string gameId)
        {
            if (gamesRepo.ContainsKey(gameId))
            {
                GameInstance game = GetGame(gameId);
                game.Status.CurrentBoard.Error = SudokuErrorEnum.OK;
                return game.Status;
            }
            else
            {
                GameStatus status = new GameStatus
                {
                    CurrentBoard = new GameBoard
                    {
                        Error = SudokuErrorEnum.InvalidGameID
                    }
                };
                return status;
            }
        }

        public GameBoard ShowFinishedBoard(string gameId)
        {
            if (gamesRepo.ContainsKey(gameId))
            {
                GameInstance instance = GetGame(gameId);
                int[,] intBoard = CloneBoard(instance.Status.CurrentBoard.Board);
                intBoard = SolveEntireBoard(intBoard);
                return new GameBoard
                {
                    Board = intBoard,
                    Error = SudokuErrorEnum.OK
                };
            }
            else
            {
                //make sure a copy of the board is filled, not actual board
                return new GameBoard
                {
                    Error = SudokuErrorEnum.InvalidGameID
                };
            }
        }

        public GameResult SetBoardValue(string gameId, BoardMove value, bool checkValue)
        {
            if (GetGame(gameId).Config.InitBoard[value.Row, value.Column] == 0)
            {
                GetGame(gameId).Status.CurrentBoard.Board[value.Row, value.Column] = value.Value;
                GetGame(gameId).Status.LastMove = value;
                //check if board is finished
                if (GetGame(gameId).Status.CurrentBoard.Board == ShowFinishedBoard(gameId).Board)
                    GetGame(gameId).Status.GameCompleted = true;
                else
                    GetGame(gameId).Status.GameCompleted = false;

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
                    Error = SudokuErrorEnum.InvalidMove
                };
            }
        }

        public List<int> GetPossibleBoardValues(string gameId, BoardMove pos)
        {
            GameInstance g;

            if (gamesRepo.TryGetValue(gameId, out g))
            {
                List<int> list = GetPossibleValuesForRowCol(g.Status.CurrentBoard.Board, pos.Row, pos.Column);
                return list;
            }
            else
            {
                //what should happen if the value fails?
                Console.WriteLine("THERE is A PROBLEM HERE");
                return new List<int> { };
            }
        }

        public BoardMove GetPossibleBoardMove(string gameId)
        {
            int value = 0;
            int numTries = 0;
            int currRow = -1, currCol = -1;

            //set board equal to specific game board
            //THIS WILL CAUSE PROBLEMS HOW DO I CLONE PROPERLY
            int[,] board = CloneBoard(GetGame(gameId).Status.CurrentBoard.Board);

            do
            {
                PickRandomBlankSpace(board, out int row, out int col);
                currRow = row;
                currCol = col;
                value = GetValueForRowCol(GetPossibleValuesForRowCol(board, row, col));
                if (value == 0)
                {
                    value = GetValueForSquare(board, row, col);
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
                        Error = SudokuErrorEnum.Timeout
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
            return completedBoard;
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

        public int[,] MakeBoard(int difficulty)
        {
            int[,] board = new int[boardSize, boardSize];
            board = FillBoard(board);
            board = DeleteSpaces(board, difficulty);
            return board;
        }

        private int[,] FillBoard(int[,] board)
        {
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

        private int[,] DeleteSpaces(int[,] board, int difficulty)
        {
            if (difficulty == 1)
            {
                for (int numDeletions = 0; numDeletions < numGivensEasy; numDeletions++)
                {
                    DeleteSpace(board);
                }
            }

            if (difficulty == 2)
            {
                for (int numDeletions = 0; numDeletions < numGivensRegular; numDeletions++)
                {
                    DeleteSpace(board);
                }
            }

            if (difficulty == 3)
            {
                for (int numDeletions = 0; numDeletions < numGivensHard; numDeletions++)
                {
                    DeleteSpace(board);
                }
            }
            return board;
        }

        private void DeleteSpace(int[,] board)
        {

            int numFails = 0;
            int[,] copyBoard = CloneBoard(board);
            do
            {
                PickRandomSpace(board, out int row, out int col);
                int originalSpace = copyBoard[row, col];
                copyBoard[row, col] = 0;

                copyBoard = SolveEntireBoard(copyBoard);

                if (BoardIsSolved(copyBoard))
                {
                    numFails = 0;
                    board[row, col] = 0;
                    return;
                }
                else
                {
                    numFails++;
                    if (numFails > boardSize)
                    {
                        return;
                    }
                }

            } while (!BoardIsSolved(copyBoard));
            return;
        }

        private void PickRandomSpace(int[,] board, out int row, out int col)
        {
            Random random = new Random();
            do
            {
                row = random.Next(0, boardSize);
                col = random.Next(0, boardSize);
            } while (board[row, col] == 0);
        }

        private void PickRandomBlankSpace(int[,] board, out int row, out int col)
        {
            Random random = new Random();
            do
            {
                row = random.Next(0, boardSize);
                col = random.Next(0, boardSize);
            } while (board[row, col] != 0);
        }
    }
}

