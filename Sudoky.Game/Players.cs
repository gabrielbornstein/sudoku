using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using System.Threading.Tasks;

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
                //DeleteCloudPlayer(playerId).Wait();
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
                //UpdateCloudPlayer(playerId, pl).Wait();
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
            //UpdateCloudPlayer(player.PlayerId, player).Wait();

            return player;
        }

        public Player GetPlayer(string playerId)
        {
            Player pl = GetPlayerInfo(playerId);
            if (pl != null)
            {
                return pl;
            }
            else
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
            IDPlayerDict.TryGetValue(playerId, out tmpPlayer);
            if (tmpPlayer == null)
            {
                //tmpPlayer = GetCloudPlayer(playerId).Result;
            }
            if (tmpPlayer == null)
            {
                return null;
            }
            return tmpPlayer;
            //return (IDPlayerDict.TryGetValue(playerId, out tmpPlayer)) ? tmpPlayer : null;
        }

        static private async Task DeleteCloudPlayer(string playerId)
        {
            FirestoreDb db = InitializeDataBase();
            DocumentReference reference = db.Collection("players").Document(playerId);
            await reference.DeleteAsync();
        }

        static private async Task UpdateCloudPlayer(string playerId, Player player)
        {
            FirestoreDb db = InitializeDataBase();
            DocumentReference reference = db.Collection("players").Document(playerId);
            await reference.SetAsync(player);
        }

        static private async Task<Player> GetCloudPlayer(string playerId)
        {
            FirestoreDb db = InitializeDataBase();
            DocumentReference reference = db.Collection("players").Document(playerId);
            DocumentSnapshot snapshot = await reference.GetSnapshotAsync();
            Player player = snapshot.ConvertTo<Player>();

            return player;
        }
    }
}
