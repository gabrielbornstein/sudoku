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

        private GridValueEnum[,] CreateEmptyBoard()
        {
            GridValueEnum[,] board = new GridValueEnum[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    board[i, j] = GridValueEnum.Blank;
                }
            }

            return board;
        }

        private GridValueEnum[,] CloneBoard(GridValueEnum[,] board)
        {
            GridValueEnum[,] clone = new GridValueEnum[boardSize, boardSize];
            for(int i=0;i<boardSize; i++)
            {
                for(int j=0;j<boardSize;j++)
                {
                    clone[i, j] = board[i, j];
                }
            }
            return clone;
        }

        public GridValueEnum[,] SolveEntireBoard(GridValueEnum[,] board)
        {
            GridValueEnum[,] completedBoard = CloneBoard(board);

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
                        if (completedBoard[i, j] == GridValueEnum.Blank)
                        {
                            currNumBlanks++;
                            GridValueEnum value = GetGridValue(board, i, j);
                            if (value == GridValueEnum.Blank)
                            {
                                completedBoard[i, j] = GetValueForSquare(board, i, j);
                            }
                            else
                            {
                                completedBoard[i, j] = value;
                            }
                        }
                    }
                }
            } while (!BoardIsSolved(board) && currNumBlanks != prevNumBlanks);

            return (BoardIsSolved(board)) ? completedBoard : null;
        }

        public GridValueEnum GetGridValue(GridValueEnum[,] board, int row, int col)
        {
            GridValueEnum mask = GetBitMaskForRow(board, row);
            mask = mask | GetBitMaskForColumn(board, col);
            mask = mask | GetBitMaskForGrid(board, row / gridSize, col / gridSize);
            return GetValueForRowCol(mask);
        }

        public GridValueEnum GetValueForSquare(GridValueEnum[,] board, int row, int col)
        {
            GridValueEnum mask = GetPossibleValuesForRowCol(board, row, col);
            for (int i = 1; i < boardSize + 1; i++)
            {
                if (((int)mask & 1 << i) != 0)
                {
                    if (!IsValuePresentInOtherSquares(board, row, col, (GridValueEnum)(1 << i)))
                    {
                        return (GridValueEnum)(1 << i);
                    }
                }
            }
            return GridValueEnum.Blank;
        }

        private bool BoardIsSolved(GridValueEnum[,] board)
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

        private GridValueEnum GetBitMaskForRow(GridValueEnum[,] board, int row)
        {
            GridValueEnum mask = 0;
            for (int i = 0; i < boardSize; i++)
            {
                mask |= board[row, i];
            }
            return mask;
        }

        private GridValueEnum GetBitMaskForColumn(GridValueEnum[,] board, int col)
        {
            GridValueEnum mask = 0;
            for (int i = 0; i < boardSize; i++)
            {
                mask |= board[i, col];
            }
            return mask;
        }

        private GridValueEnum GetBitMaskForGrid(GridValueEnum[,] board, int anchorRow, int anchorCol)
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

        public GridValueEnum GetPossibleValuesForRowCol(GridValueEnum[,] board, int row, int col)
        {
            GridValueEnum mask = GetBitMaskForRow(board, row);
            mask = mask | GetBitMaskForColumn(board, col);
            mask = mask | GetBitMaskForGrid(board, row / gridSize, col / gridSize);
            mask = ~(mask) & (GridValueEnum)0x3ff;
            return mask;
        }

        private bool IsValuePresentInOtherSquares(GridValueEnum[,] board, int row, int col, GridValueEnum value)
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
                            GridValueEnum mask = GetPossibleValuesForRowCol(board, currRow, currCol);
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

        public GameInstance GetGame(string gameId)
        {
            GameInstance tmpGameInstance;
            return (gamesRepo.TryGetValue(gameId, out tmpGameInstance)) ? tmpGameInstance : null;
        }

        public GameInstance CreateNewGame(GameConfig config)
        {
            GridValueEnum[,] board = CreateEmptyBoard();

            GameInstance game = new GameInstance
            {
                Status = new GameStatus
                {
                    CurrentBoard = new GameBoard
                    {
                        Board = MakeBoard(board, config.Difficulty)
                    },
                    NextPlayerId = config.Player2Id,
                    LastMove = new BoardMove(),
                    GamePaused = false,
                },
                Config = config,
                GameId = Guid.NewGuid().ToString()
            };
            game.Config.InitBoard = (int[,])game.Status.CurrentBoard.Board.Clone();
            gamesRepo.TryAdd(game.GameId, game);
            return game;
        }

        public int[,] MakeBoard(GridValueEnum[,] board, int difficulty)
        {
            FillBoard(board);
            DeleteSpaces(board, difficulty);
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

        public GridValueEnum[,] FillBoard(GridValueEnum[,] board)
        {
            Random rnd = new Random();
            for (int i = 0; i < boardSize; i++)
            {
                List<GridValueEnum> possibleValues = new List<GridValueEnum>();
                GridValueEnum possibleValuesMask = GetPossibleValuesForRowCol(board, 0, i);
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
            ShiftBoard(board, 3, 1);
            ShiftBoard(board,3, 2);
            ShiftBoard(board, 1, 3);
            ShiftBoard(board, 3, 4);
            ShiftBoard(board, 3, 5);
            ShiftBoard(board, 1, 6);
            ShiftBoard(board, 3, 7);
            ShiftBoard(board,3, 8);
            return board;
        }

        private void ShiftBoard(GridValueEnum[,] board, int ShiftSize, int row)
        {
            for (int i = 0; i < boardSize; i++)
            {
                if (i + ShiftSize >= boardSize)
                    board[row, i] = board[row - 1, (i + ShiftSize) % boardSize];
                else
                    board[row, i] = board[row - 1, i + ShiftSize];
            }
        }

        public GridValueEnum[,] DeleteSpaces(GridValueEnum[,] board, int difficulty)
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

        private void DeleteSpace(GridValueEnum[,] board)
        {

            GridValueEnum[,] copyBoard = (GridValueEnum[,])board.Clone();
            int numFails = 0;

            do
            {
                PickRandomSpace(board, out int row, out int col);
                GridValueEnum originalSpace = board[row, col];
                board[row, col] = GridValueEnum.Blank;

                SolveEntireBoard(board);

                if (BoardIsSolved(board))
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

            } while (!BoardIsSolved(board));
            return;
        }

        private void PickRandomSpace(GridValueEnum[,] board, out int row, out int col)
        {
            Random random = new Random();
            do
            {
                row = random.Next(0, boardSize);
                col = random.Next(0, boardSize);
            } while (board[row, col] == GridValueEnum.Blank);
        }

        private void PickRandomBlankSpace(GridValueEnum[,] board, out int row, out int col)
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

        //This method can be simplified
        public GameBoard ShowFinishedBoard(string gameId)
        {
            if (gamesRepo.ContainsKey(gameId))
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
                int[,] intBoard = new int[boardSize, boardSize];
                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        intBoard[i, j] = CastGridValToInt(gridValueBoard[i, j]);
                    }
                }
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

            if(gamesRepo.TryGetValue(gameId, out g)) { 

                List<int> list = new List<int>();
                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        board[i, j] = CastIntToGridValue(g.Status.CurrentBoard.Board[i, j]);
                    }
                }

                GridValueEnum mask = GetPossibleValuesForRowCol(board, pos.Row, pos.Column);
                for (int i = 0; i < boardSize + 1; i++)
                {
                    if (((int)mask & (1 << i)) != 0)
                    {
                        list.Add(CastGridValToInt((GridValueEnum)(1 << i)));
                    }

                }
                return list;
            } else
            {

            }
           
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
                    board[i, j] = CastIntToGridValue(GetGame(gameId).Status.CurrentBoard.Board[i, j]);
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
    }
}

