using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GEB.Sudoku;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SudokuAPI.playerController
{
    [Route("api/sudoku/players")]
    [ApiController]
    public class PlayersController : Controller
    {
        //how do i add query?
        [HttpGet("{playerId}")]
        public JsonResult GetPlayerInfo(string playerId)
        {
            Game tmpGame = new Game();
            return Json(tmpGame.GetPlayer(playerId));
        }

        [HttpGet("create/{name}")]
        public JsonResult RegisterPlayer(string name)
        {
            Game tmpGame = new Game();
            return Json(tmpGame.RegisterPlayer(name));
        }

        /*
        [HttpDelete("{playerId}")]
        public void RemovePlayer()
        {
            GetPlayerInfo(playerId).DeletePlayer();
        }

        [HttpPut("{playerId}/rename/{name}")]
        public JsonResult NamePlayer()
        {
            RenamePlayer(playerId, name);
            return GetPlayerInfo(playerId);
        }
        */
    }
}
