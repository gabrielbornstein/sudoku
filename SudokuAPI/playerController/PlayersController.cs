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

        //needs to have a query added
        [HttpPost]
        public JsonResult RegisterPlayer(string name)
        {
            Game tmpGame = new Game();
            return Json(tmpGame.RegisterPlayer(name));
        }

        [HttpDelete("{playerId}")]
        public void RemovePlayer(string playerId)
        {
            Game tmpGame = new Game();
            tmpGame.DeletePlayer(playerId);
        }

        [HttpPut("{playerId}/rename/{name}")]
        public JsonResult NamePlayer(string playerId, string name)
        {
            Game tmpGame = new Game();
            tmpGame.RenamePlayer(playerId, name);
            return GetPlayerInfo(playerId);
        }
    }
}
