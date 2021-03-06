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
        [ProducesResponseType(typeof(GameInstance), 200)]
        public JsonResult CreateAGame([FromBody] GameConfig config)
        {
            return Json(Sudoku.GetSudokuService().CreateNewGame(config));
        }

        [HttpDelete("{gameId}")]
        [ProducesResponseType(typeof(GameResult), 200)]
        public void StopGame(string gameId)
        {
            Sudoku.GetSudokuService().CancelGame(gameId);
        }

        // Possible Actions: pause | play
        [HttpPut("{gameId}/action/{pauseOrPlayAction}")]
        [ProducesResponseType(typeof(GameResult), 200)]
        public JsonResult PauseOrPlayGame(string gameId, string pauseOrPlayAction)
        {
            return Json(Sudoku.GetSudokuService().PauseGame(gameId, (pauseOrPlayAction.ToLower() == "pause")));
        }

        [HttpGet("player/{playerId}")]
        [ProducesResponseType(typeof(GameList), 200)]
        public JsonResult GetGamesByPlayerID(string playerId)
        {
            return Json(Sudoku.GetSudokuService().GamesByPlayer(playerId));
        }

        [HttpGet("{gameId}/status")]
        [ProducesResponseType(typeof(GameStatus), 200)]
        public JsonResult CurrentGameStatus(string gameId)
        {
            return Json(Sudoku.GetSudokuService().GetGame(gameId).Status);
        }

        [HttpGet("{gameId}/completed")]
        [ProducesResponseType(typeof(GameBoard), 200)]
        public JsonResult CheckGameCompleted(string gameId)
        {
            return Json(Sudoku.GetSudokuService().GetGame(gameId).Status.CurrentBoard);
        }

        [HttpPut("{gameId}/move")]
        [ProducesResponseType(typeof(GameResult), 200)]
        public JsonResult MakeAMove(string gameId, BoardMove move)
        {
            return Json(Sudoku.GetSudokuService().SetBoardValue(gameId, move));
        }

        [HttpGet("{gameId}/next")]
        [ProducesResponseType(typeof(BoardMove), 200)]
        public JsonResult GiveAPossibleMove(string gameId)
        {
            return Json(Sudoku.GetSudokuService().GetPossibleBoardMove(gameId));
        }
    }
}