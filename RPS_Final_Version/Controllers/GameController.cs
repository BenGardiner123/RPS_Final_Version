using Microsoft.AspNetCore.Mvc;
using RPS_Final_Version.Models;
using RPS_Final_Version.Models.ViewModels;
using RPS_Final_Version.ultities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RPS_Final_Version.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        ///swagger endpoint because i always forget to add it
        /// https://localhost:7066/swagger/index.html
        private readonly rock_paper_scissorsContext _context;

        public IConfiguration Configuration { get; }
        //create a new instance of the AI selection class
        AiSelection aiSelection = new AiSelection();

        public GameController(rock_paper_scissorsContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;

        }

        // POST api/Game/StartGame
        [HttpPost("StartGame")]
        public ActionResult
            Post([FromBody] GameCheckRequestModel beginGame)
        {
            var debuggerCheck = beginGame;

            //make user incoming model is not null
            if (beginGame.Username == null || beginGame.DateTimeStarted == DateTime.MinValue || beginGame.roundLimit == 0)
            {
                return BadRequest("Please enter a username, datetime, and round limit");
            }

            try
            {
                //check if the player exists  //check if the player exists
                var player = _context.Players.FirstOrDefault(p => p.Username == beginGame.Username);
                if (player == null)
                {
                    return BadRequest("Player does not exist");
                }

                //create a new game and add it to the database
                var game = new Game
                {
                    Gamecode = Guid.NewGuid().ToString(),
                    GamerWinner = "",
                    Roundlimit = beginGame.roundLimit,
                    Datetimestarted = beginGame.DateTimeStarted,
                    Datetimeended = DateTime.MinValue,
                    PlayerOne = beginGame.Username,
                    PlayerTwo = "The AI Bot",
                    PlayerOneNavigation = player,
                    PlayerTwoNavigation = _context.Players.FirstOrDefault(p => p.Username == "The AI Bot")

                };

                //insert the game into the database
                _context.Games.Add(game);
                _context.SaveChanges();


                //if save changes succesful then return the gamecheck response model
                return Ok();




            }
            catch (Exception ex)
            {
                var error = ex;

                return BadRequest($"{BadRequest().StatusCode} : {ex.Message}");
            }
        }

        [HttpPost("postSelection")]
        public ActionResult<GameSelectionResponseModel> PostSelection(GameSelectionModel beginGame)
        {
            //make user incoming model is not null
            if (beginGame.Username == null || beginGame.DateTimeStarted == DateTime.MinValue || beginGame.roundLimit == 0 || beginGame.PlayerChoice == null)
            {
                return BadRequest("Please enter a username, datetime, and round limit");
            }

            try
            {
                //check if the player and DateTimeStarted exists
                var player = _context.Players.FirstOrDefault(p => p.Username == beginGame.Username);
                if (player == null)
                {
                    return BadRequest("Player does not exist");
                }

                var game = _context.Games.FirstOrDefault(g => g.Datetimestarted == beginGame.DateTimeStarted && g.PlayerOne == beginGame.Username);
                if (game == null)
                {
                    return BadRequest("Game does not exist");
                }

                var gameIdCheck = game.Gameid;


                //create a new round and update it
                var round = new Round
                {
                    Gameid = game.Gameid,
                    Roundnumber = beginGame.roundCounter,
                    PlayerOneChoice = beginGame.PlayerChoice,
                    PlayerTwoChoice = aiSelection.AiChoice()
                };

                //add the round to the database
                _context.Rounds.Add(round);
                _context.SaveChanges();

                //if save changes succesful then calulate the winner and return the information to the front end
                var roundOutcome = aiSelection.CalculateGameWinner(round.PlayerOneChoice, round.PlayerTwoChoice);

                return Ok(new GameSelectionResponseModel
                {
                    PlayerTwoChoice = round.PlayerTwoChoice,
                    outcome = roundOutcome
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"{BadRequest().StatusCode} : {ex.Message}");
            }
        }


    }

}
