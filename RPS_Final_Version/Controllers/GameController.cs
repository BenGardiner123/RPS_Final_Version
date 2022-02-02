using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        calcWinner calcWinner = new calcWinner();

        public GameController(rock_paper_scissorsContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;

        }

        // POST api/Game/StartGame
        [HttpPost("StartGame")]
        public ActionResult<GameCheckResponseModel> Post([FromBody] GameCheckRequestModel beginGame)
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

                var createAiChoice = aiSelection.AiChoice();
                var whoWonThisRound = aiSelection.CalculateRoundWinner(beginGame.PlayerChoice, createAiChoice);

                //create a new round for the game and update it
                var round = new Round
                {
                    Gameid = game.Gameid,
                    Roundnumber = beginGame.roundCounter,
                    PlayerOneChoice = beginGame.PlayerChoice,
                    PlayerTwoChoice = createAiChoice,
                    Winner = whoWonThisRound
                };

                //add the round to the database
                _context.Rounds.Add(round);
                _context.SaveChanges();


                //check if the roundCounter is equal to the round limit and if it is then we update the game record in the DB
                if (beginGame.roundCounter == beginGame.roundLimit)
                {

                    var passTheRoundInfo = _context.Rounds.Where(r => r.Gameid == game.Gameid).ToList();

                    var gameWinner = calcWinner.CalulateGameWinner(passTheRoundInfo);

                    //update the db with the game winner where the gameid is equal to the gameid
                    var updateGame = _context.Games.FirstOrDefault(g => g.Gameid == game.Gameid);

                    //null check the updateGame
                    if (updateGame != null)
                    {
                        updateGame.GameWinner = gameWinner;
                        updateGame.Datetimeended = DateTime.Now;
                        _context.SaveChanges();
                    }

                    //we still want to return the round winner and then another endpoint will take care of the rest
                    return Ok(new GameSelectionResponseModel
                    {
                        PlayerTwoChoice = round.PlayerTwoChoice,
                        outcome = round.Winner
                    });

                }
                else
                {
                    //if it is not then return the game winner and the next round counter
                    return Ok(new GameSelectionResponseModel
                    {
                        PlayerTwoChoice = round.PlayerTwoChoice,
                        outcome = round.Winner
                    });
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"{BadRequest().StatusCode} : {ex.Message}");
            }
        }


        [HttpPost("GameResult")]
        public async Task<ActionResult<GameResultResponseModel>> GetGameResult(string username, DateTime dateTimeStarted)
        {
            var game = await _context.Games.FirstOrDefaultAsync(x => x.Datetimestarted == dateTimeStarted && x.PlayerOne == username);

            if (game == null)
            {
                return NotFound();
            }

            //try catch to get the rounds for the game
            try
            {
                var output = _context.Rounds.Where(r => r.Gameid == game.Gameid).ToList();

                var rounds = output.Select(r => new GameResultResponse_RoundModel
                {
                    RoundNumber = r.Roundnumber,
                    PlayerOneChoice = r.PlayerOneChoice,
                    PlayerTwoChoice = r.PlayerTwoChoice,
                    Winner = r.Winner
                }).ToList();

                //return the game result
                return Ok(new GameResultResponseModel
                {
                    GameWinner = game.GameWinner,
                    Rounds = rounds
                });

            }
            catch (Exception ex)
            {
                return BadRequest($"{BadRequest().StatusCode} : {ex.Message}");
            }

        }

    }

}
