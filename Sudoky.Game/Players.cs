using System;
using System.Collections.Generic;

namespace GEB.Sudoku
{
    public partial class Sudoku : IPlayers
    {
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
                Error = (pl != null) ? SudokuErrorEnum.OK : SudokuErrorEnum.InvalidPlayerID,
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
                Error = (pl != null) ? SudokuErrorEnum.OK : SudokuErrorEnum.InvalidPlayerID,
                Result = (pl != null)
            };
        }

        public Player RegisterPlayer(string name)
        {
            Player player = new Player()
            {
                PlayerName = name,
                GamesPlayed = 0,
                GamesFinished = 0,
                Score = 0,
                Error = SudokuErrorEnum.OK
            };

            //generate user ID
            player.PlayerId = Guid.NewGuid().ToString();
            IDPlayerDict.TryAdd(player.PlayerId, player);

            return player;
        }

        public Player GetPlayer(string playerId)
        {
            Player pl = GetPlayer(playerId);
            if (pl != null)
            {
                return pl;
            } else
            {
                return new Player()
                {
                    Error = (pl != null) ? SudokuErrorEnum.OK : SudokuErrorEnum.InvalidPlayerID,
                    ErrorDescription = string.Format("Invalid Player ID {0}", playerId)
                };
            };
        }

        private Player GetPlayerInfo(string playerId)
        {
            Player tmpPlayer;
            return (IDPlayerDict.TryGetValue(playerId, out tmpPlayer)) ? tmpPlayer : null;
        }

    }
}
