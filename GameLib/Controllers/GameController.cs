using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using gameLib.Repos;
using gameLib.Models;
using gameLib.Dtos;

namespace gameLib.Controllers
{
    [ApiController]
    [Route("games")]
    public class GamesController : ControllerBase
    {
        public GamesController(GameDBInterface gameRepo)
        {
            this.gameRepo = gameRepo;
        }

        private readonly GameDBInterface gameRepo;


        [HttpGet("{title}")]
        public ActionResult<GameDto> LoadGame(string title) {
            var game = gameRepo.LoadByTitle(title);
            if (game == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(game.AsDto());
        }

        [HttpGet]
        public ActionResult<IEnumerable<GameDto>> LoadAllGames([FromQuery] PageSettings pageSettings)
        {
            var games = gameRepo.LoadAll(pageSettings).Select(game => game.AsDto());
            if (games.Count() < 1)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(games);
        }

        [HttpPost("search")]
        public ActionResult<IEnumerable<GameDto>> SearchGames([FromBody] List<string> titles, [FromQuery] PageSettings pageSettings)
        {
            var games = gameRepo.Search(titles, pageSettings).Select(game => game.AsDto());
            if (games.Count() < 1)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(games);
        }

        [HttpPost]
        public ActionResult<bool> AddGames(List<GameDto> games)
        {
            List<Game> toSave = games.Cast<Game>().ToList();
            bool success = gameRepo.Add(toSave);
            if (!success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "falied to add game");
            }
            return Ok(true);
        }
    }
}
