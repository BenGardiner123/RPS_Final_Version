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
        public ActionResult <GameCheckResponseModel>Post([FromBody] GameCheckRequestModel beginGame)
        {
            var gameCheck = beginGame;

            //make user incoming model is not null values that might stuff the process up
            if (beginGame.Username == null || beginGame.DateTimeStarted == DateTime.MinValue || beginGame.roundLimit == 0)
            {
                return BadRequest("Please enter a username, datetime, and round limit");
            }

            try
            {
                //check if the player exists 
                var player = _context.Players.FirstOrDefault(p => p.Username == beginGame.Username);
                if (player == null)
                {
                    return BadRequest("Player does not exist");
                }

                //get the game id to be used in the next step
                var gameSetup = _context.Games.FirstOrDefault(g => g.Gameid == 1);
                if (gameSetup == null)
                {
                    //create a new game with id set to 1 if none exists
                    var newGame = new Game
                    {
                        Gameid = 1,
                        Gamecode = Guid.NewGuid().ToString(),
                        Datetimestarted = beginGame.DateTimeStarted,
                        Roundlimit = beginGame.roundLimit,
                        PlayerOne = beginGame.Username,
                        PlayerTwo = "The AI Bot"
                    };
                    _context.Games.Add(newGame);
                    _context.SaveChanges();
                }
                else if (gameSetup.Gameid >= 1)
                {
                      //other create a new game with id set to +1 whatever the current max game id is
                    var newGame = new Game
                    {
                        Gameid = _context.Games.Max(g => g.Gameid) + 1,
                        Gamecode = Guid.NewGuid().ToString(),
                        Datetimestarted = beginGame.DateTimeStarted,
                        Roundlimit = beginGame.roundLimit,
                        PlayerOne = beginGame.Username,
                        PlayerTwo = "The AI Bot"
                    };
                    _context.Games.Add(newGame);
                    _context.SaveChanges();
                  
                }
                else
                {
                    return BadRequest("Game already exists");
                }
               

                //if save changes succesful then return the gamecheck response model
                return Ok(new GameCheckResponseModel
                {
                    Username = beginGame.Username,
                    roundLimit = beginGame.roundLimit,
                    DateTimeStarted = beginGame.DateTimeStarted,
                    roundCounter = beginGame.roundCounter   
                });

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
            if (beginGame.Username == null || beginGame.DateTimeStarted == DateTime.MinValue || 
                beginGame.roundLimit == 0 || beginGame.PlayerChoice == null)
            {
                return BadRequest("Please enter a username, datetime, and round limit");
            }

            try
            {
                //check if the player and game exist
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


                //create a new round for the game and update it
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
