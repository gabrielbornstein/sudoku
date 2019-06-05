﻿using System;
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

            return Json(Sudoku.GetSudokuService().CreateNewGame(config));
        }

        [HttpDelete("{gameId}")]
        public void StopGame(string gameId)
        {
            Sudoku.GetSudokuService().CancelGame(gameId);
        }

        // Possible Actions: pause | play
        [HttpPut("{gameId}/action/{action}")]
        public JsonResult PauseOrPlayGame(string gameId, string action)
        {
            return Json(Sudoku.GetSudokuService().PauseGame(gameId, (action.ToLower() == "pause")));
        }

        [HttpGet("{gameId}/status")]
        public JsonResult CurrentGameStatus(string gameId)
        {
            return Json(Sudoku.GetSudokuService().GetGame(gameId).Status);
        }

        [HttpGet("{gameId}/completed")]
        public JsonResult CheckGameCompleted(string gameId)
        {
            return Json(Sudoku.GetSudokuService().GetGame(gameId).Status.CurrentBoard);
        }

        [HttpPut("{gameId}/move")]
        public JsonResult MakeAMove(string gameId, BoardMove move)
        {
            //what to do with third parameter
            if (Sudoku.GetSudokuService().GetGame(gameId).Config.EnableAssistMode == true)
            {
                bool listHasValue = false;
                foreach (int x in Sudoku.GetSudokuService().GetPossibleBoardValues(gameId, move))
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
            return Json(Sudoku.GetSudokuService().SetBoardValue(gameId, move, false));
        }

        [HttpGet("{gameId}/next")]
        public JsonResult GiveAPossibleMove(string gameId)
        {
            return Json(Sudoku.GetSudokuService().GetPossibleBoardMove(gameId));
        }
    }
}