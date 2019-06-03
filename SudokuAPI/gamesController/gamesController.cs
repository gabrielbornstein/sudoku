using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GEB.Sudoku;

namespace SudokuAPI.playerController
{
    [Route("api/sudoku/games")]
    [ApiController]
    class GameController : Controller
    {
        //add a query for config
        [HttpPost]
        public JsonResult CreateAGame(GameConfig config)
        {
            Game tmpGame = new Game();
            return Json(tmpGame.CreateNewGame(config));
        }

        [HttpDelete("{gameId}")]
        public void StopGame(string gameId)
        {
            Game tmpGame = new Game();
            tmpGame.CancelGame(gameId);
        }

        [HttpPut("{gameId}/action/{pause|play}")]
        public JsonResult PauseOrPlayGame(string gameId, bool pause)
        {
            Game tmpGame = new Game();
            return Json(tmpGame.PauseGame(gameId, pause));
        }

        [HttpGet("{gameId}/status")]
        public JsonResult CurrentGameStatus(string gameId)
        {
            Game tmpGame = new Game();
            return Json(tmpGame.GetGame(gameId).Status);
        }

        [HttpGet("{gameId}/completed")]
        public JsonResult CheckGameCompleted(string gameId)
        {
            Game tmpGame = new Game();
            return Json(tmpGame.GetGame(gameId).Status.CurrentBoard);
        }

        [HttpPut("{gameId}/move")]
        public JsonResult MakeAMove(string gameId, BoardMove move, bool? validate)
        {
            Game tmpGame = new Game();
            //what to do with third parameter
            if (validate == true)
            {
                bool listHasValue = false;
                foreach (int x in tmpGame.GetPossibleBoardValues(gameId, move))
                {
                    if (x == move.Value)
                        listHasValue = true;
                }
                if (!listHasValue)
                    return Json(new GameResult
                    {
                        Result = false,
                        Error = GameErrorEnum.InvalidMove
                    });
            }
            return Json(tmpGame.SetBoardValue(gameId, move, false));
        }

        [HttpGet("{gameId}/next")]
        public JsonResult GiveAPossibleMove(string gameId)
        {
            Game tmpGame = new Game();
            return Json(tmpGame.GetPossibleBoardMove(gameId));
        }
    }
}