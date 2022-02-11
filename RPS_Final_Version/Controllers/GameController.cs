using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPS_Final_Version.Models;
using RPS_Final_Version.Models.ViewModels;
using RPS_Final_Version.ultities;
using System.Net.Http;
using System.Threading.Tasks;

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

                //get the game code to be used in the next step
                var gameCode = _context.Games.FirstOrDefault(g => g.Gameid == 1);


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

        // create a get mthod that take username and datetime started as params returns the gamecode
        // GET api/Game/GetGameCode/{username}/{datetime}
        [HttpPost("GameCode")]
        public ActionResult<GameCheckResponseModel> GetGameCode(GameCodeRequestModel requestModel)
        {
            //make sure the username and datetime are not null
            if (requestModel.Username == null || requestModel.DateTimeStarted == DateTime.MinValue)
            {
                return BadRequest("Please enter a username and/or datetime");
            }
            
        
            //get gameCode from the database using the username and datetime
            var game = _context.Games.FirstOrDefault(g => g.PlayerOne == requestModel.Username && g.Datetimestarted == requestModel.DateTimeStarted);

            //if the game is null then return bad request
            if (game == null)
            {
                return BadRequest("Game does not exist");
            }

            var gameCode = game.Gamecode;

            //return the gamecode
            return Ok(new GameCodeResponseModel
            {

                GameCode = gameCode
            });

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

                //we still want to return the round winner and then another endpoint will take care of the rest
                return Ok(new GameSelectionResponseModel
                {
                    PlayerTwoChoice = round.PlayerTwoChoice,
                    outcome = round.Winner
                });

            }
            catch (Exception ex)
            {
                return BadRequest($"{BadRequest().StatusCode} : {ex.Message}");
            }
        }

        //create a post method that will calulate the winner of the game and update the game table
        // POST api/Game/CalculateWinner
        [HttpPost("CalculateWinner")]
        public async Task<ActionResult<GameWinnerResponseModel>> CalculateWinner([FromBody] string gameCode)
        {
            //make sire the gamecode is not null
            if (gameCode == null)
            {
                return BadRequest("Please enter a gamecode");
            }
            try
            {
                //get the game from the database

                var game = await _context.Games.FirstOrDefaultAsync(g => g.Gamecode == gameCode);

                if (game == null)
                {
                    return BadRequest("Game does not exist");
                }

                calcWinner calcWinner = new calcWinner();

                //get the round limit from the game 
                var roundLimit = game.Roundlimit;
                //get the roundnumber from the game
                var roundNumber = game.Rounds.Max(r => r.Roundnumber);

                if (roundNumber == 0 || roundNumber < roundLimit)
                {
                    return BadRequest("Game has not started");
                }

                else if (roundNumber == roundLimit)
                {
                    var passTheRoundInfo = await _context.Rounds.Where(r => r.Gameid == game.Gameid).ToListAsync();

                    var gameWinner = calcWinner.CalulateGameWinner(passTheRoundInfo);

                    //update the game table with the winner
                    game.GameWinner = gameWinner;
                    game.Datetimeended = DateTime.Now;
                    await _context.SaveChangesAsync();

                    return Ok(new GameWinnerResponseModel
                    {
                        GameWinner = gameWinner
                    });

                }
                else
                {
                    return BadRequest("Game has not ended");
                }




            }
            catch (Exception ex)
            {
                return BadRequest($"{BadRequest().StatusCode} : {ex.Message}");
            }

        }


        //GET api/Game/GameResult/{gamecode}
        [HttpGet("GameResult")]
        public ActionResult<GameWinnerResponseModel> GetGameWinner([FromQuery]string gamecode)
        {
            //make sure the gamecode is not null
            if (gamecode == null)
            {
                return BadRequest("Please enter a gamecode");
            }

            //get the game from the database using the gamecode
            var game = _context.Games.FirstOrDefault(g => g.Gamecode == gamecode);

            //if the game is null then return bad request
            if (game == null)
            {
                return BadRequest("Game does not exist");
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
                    Winner = game.GameWinner,
                    Rounds = rounds
                });

            }


            catch (Exception ex)
            {
                return BadRequest($"{BadRequest().StatusCode} : {ex.Message}");
            }


        }


        public static string getWinner(DateTime dateTime, string username)
        {
            using (var context = new rock_paper_scissorsContext())
            {
                var game = context.Games.FirstOrDefault(g => g.Datetimestarted == dateTime && g.PlayerOne == username);
                if (game == null)
                {
                    return ("Game does not exist");
                }

                var round = context.Rounds.FirstOrDefault(r => r.Gameid == game.Gameid);
                if (round == null)
                {
                    return ("Round does not exist");
                }

                var winner = round.Winner;

                return winner;
            }

        }

    }

}
