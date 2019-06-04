using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GEB.Sudoku;

namespace SudokuAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : Microsoft.AspNetCore.Mvc.Controller
    {
        //add a query for config
        [HttpPost]
        public JsonResult CreateAGame([FromBody] GameConfig config)
        {
            GEB.Sudoku.Sudoku tmpGame = new GEB.Sudoku.Sudoku();
            return Json(tmpGame.CreateNewGame(config));
        }

        [HttpDelete("{gameId}")]
        public void StopGame(string gameId)
        {
            GEB.Sudoku.Sudoku tmpGame = new GEB.Sudoku.Sudoku();
            tmpGame.CancelGame(gameId);
        }

        [HttpPut("{gameId}/action/{pause|play}")]
        public JsonResult PauseOrPlayGame(string gameId, bool pause)
        {
            GEB.Sudoku.Sudoku tmpGame = new GEB.Sudoku.Sudoku();
            return Json(tmpGame.PauseGame(gameId, pause));
        }

        [HttpGet("{gameId}/status")]
        public JsonResult CurrentGameStatus(string gameId)
        {
            GEB.Sudoku.Sudoku tmpGame = new GEB.Sudoku.Sudoku();
            return Json(tmpGame.GetGame(gameId).Status);
        }

        [HttpGet("{gameId}/completed")]
        public JsonResult CheckGameCompleted(string gameId)
        {
            GEB.Sudoku.Sudoku tmpGame = new GEB.Sudoku.Sudoku();
            return Json(tmpGame.GetGame(gameId).Status.CurrentBoard);
        }

        [HttpPut("{gameId}/move")]
        public JsonResult MakeAMove(string gameId, BoardMove move, bool? validate)
        {
            GEB.Sudoku.Sudoku tmpGame = new GEB.Sudoku.Sudoku();
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
                        Error = SudokuErrorEnum.InvalidMove
                    });
            }
            return Json(tmpGame.SetBoardValue(gameId, move, false));
        }

        [HttpGet("{gameId}/next")]
        public JsonResult GiveAPossibleMove(string gameId)
        {
            GEB.Sudoku.Sudoku tmpGame = new GEB.Sudoku.Sudoku();
            return Json(tmpGame.GetPossibleBoardMove(gameId));
        }
    }
}