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
        public ActionResult<GameCheckResponseModel> Post(GameCheckRequestModel beginGame)
        {
            //make user incoming model is not null
            if (beginGame.Username == null || beginGame.DateTimeStarted == DateTime.MinValue || beginGame.roundLimit == 0)
            {
                return BadRequest("Please enter a username, datetime, and round limit");
            }

            try
            {
                //check if the player exists  //check if the player exists
                var player = _context.Players.FirstOrDefault(p => p.Username == beginGame.Username);

                //create a new game and add it to the database
                var game = new Game
                {
                    Datetimestarted = beginGame.DateTimeStarted,
                    PlayerOne = beginGame.Username,
                    PlayerTwo = "The AI Bot",
                    //create a gamecode using a GUID - this would make an ID if you wanted to come back to the game later on perhaps
                    Gamecode = Guid.NewGuid().ToString(),   
                    Roundlimit = beginGame.roundLimit
                };

                //insert the game into the database
                _context.Games.Add(game);
                _context.SaveChanges();

                
                //if save changes succesful then return the gamecheck response model
                return Ok(new GameCheckResponseModel
                {
                    Username = game.PlayerOne,
                    roundLimit = game.Roundlimit,
                    DateTimeStarted = game.Datetimestarted,
                    roundCounter = 1
                });

                

            }
            catch (Exception ex)
            {
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
