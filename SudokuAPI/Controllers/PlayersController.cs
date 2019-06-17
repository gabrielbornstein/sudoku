using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GEB.Sudoku;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SudokuAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : Microsoft.AspNetCore.Mvc.Controller
    {
        //how do i add query?
        [HttpGet("{playerId}")]
        [ProducesResponseType(typeof(Player), 200)]
        public JsonResult GetPlayerInfo(string playerId)
        {
            return Json(Sudoku.GetSudokuService().GetPlayer(playerId));
        }

        //needs to have a query added
        [HttpPost("register")]
        [ProducesResponseType(typeof(Player), 200)]
        public JsonResult RegisterPlayer([FromQuery] string name)
        {
            return Json(Sudoku.GetSudokuService().RegisterPlayer(name));
        }

        [HttpDelete("{playerId}")]
        [ProducesResponseType(typeof(GameResult), 200)]
        public void RemovePlayer(string playerId)
        {
            Sudoku.GetSudokuService().DeletePlayer(playerId);
        }

        [HttpPut("{playerId}/rename/{name}")]
        [ProducesResponseType(typeof(GameResult), 200)]
        public JsonResult NamePlayer(string playerId, string name)
        {
            Sudoku.GetSudokuService().RenamePlayer(playerId, name);
            return GetPlayerInfo(playerId);
        }
    }
}
