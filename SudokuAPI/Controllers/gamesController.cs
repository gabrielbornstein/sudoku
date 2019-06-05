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

            return Json(GEB.Sudoku.Sudoku.GetSudokuService().CreateNewGame(config));
        }

        [HttpDelete("{gameId}")]
        public void StopGame(string gameId)
        {
            GEB.Sudoku.Sudoku.GetSudokuService().CancelGame(gameId);
        }

        [HttpPut("{gameId}/action/{pause|play}")]
        public JsonResult PauseOrPlayGame(string gameId, bool pause)
        {
            return Json(GEB.Sudoku.Sudoku.GetSudokuService().PauseGame(gameId, pause));
        }

        [HttpGet("{gameId}/status")]
        public JsonResult CurrentGameStatus(string gameId)
        {
            return Json(GEB.Sudoku.Sudoku.GetSudokuService().GetGame(gameId).Status);
        }

        [HttpGet("{gameId}/completed")]
        public JsonResult CheckGameCompleted(string gameId)
        {
            return Json(GEB.Sudoku.Sudoku.GetSudokuService().GetGame(gameId).Status.CurrentBoard);
        }

        [HttpPut("{gameId}/move")]
        public JsonResult MakeAMove(string gameId, BoardMove move, bool? validate)
        {
            //what to do with third parameter
            if (validate == true)
            {
                bool listHasValue = false;
                foreach (int x in GEB.Sudoku.Sudoku.GetSudokuService().GetPossibleBoardValues(gameId, move))
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
            return Json(GEB.Sudoku.Sudoku.GetSudokuService().SetBoardValue(gameId, move, false));
        }

        [HttpGet("{gameId}/next")]
        public JsonResult GiveAPossibleMove(string gameId)
        {
            return Json(GEB.Sudoku.Sudoku.GetSudokuService().GetPossibleBoardMove(gameId));
        }
    }
}