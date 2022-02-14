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

                    //push newGame into the gameCheck response model

                    return Ok(new GameCheckResponseModel
                    {
                        GameCode = newGame.Gamecode
                    });

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

                    //return the game
                    return Ok(new GameCheckResponseModel
                    {
                        GameCode = newGame.Gamecode
                    });

                }
                else
                {
                    return BadRequest("Game already exists");
                }

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
        public async Task<ActionResult<GameWinnerResponseModel>> CalculateWinner(GameWinnerRequestModel gameCode)
        {
            var finalWinner = "";
            var roundLimit = 0;
            var roundNumber = 0;

            var output = gameCode.gameCode;

            calcWinner calcWinner = new calcWinner();

            //make sire the gamecode is not null
            if (gameCode == null)
            {
                return BadRequest("Please enter a gamecode");
            }
            try
            {
                //get the game from the database

                var game = await _context.Games.FirstOrDefaultAsync(g => g.Gamecode == gameCode.gameCode);  

                if (game == null)
                {
                    return BadRequest("Game does not exist");
                }

                //get the round limit from the game 
                roundLimit = game.Roundlimit;
                // get all the rounds from the game 
                var rounds = _context.Rounds.Where(r => r.Gameid == game.Gameid).ToList();

                //get the round number from rounds
                roundNumber = rounds.Count();

                

                if (roundNumber == roundLimit)
                {

                    //create a new using dbcontext
                    using (var db = new rock_paper_scissorsContext())
                    {
                        //get the game from the database
                        var gameToUpdate = await db.Rounds.Where(r => r.Gameid == game.Gameid).ToListAsync();

                        //get the winner from the game
                        var winner = calcWinner.CalculateGameWinner(gameToUpdate);

                        //update the game with the winner
                        finalWinner = winner;

                        //update the game table with the winner
                        game.GameWinner = finalWinner;
                        game.Datetimeended = DateTime.Now;
                        await _context.SaveChangesAsync();

                    }

                    return Ok(new GameWinnerResponseModel
                    {
                        GameWinner = finalWinner
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
        [HttpPost("GameResult")]
        public async Task<ActionResult<GameResultResponseModel>> GetGameWinner(GameWinnerRequestModel gamecode)
        {

            //make sure the gamecode is not null
            if (gamecode == null)
            {
                return BadRequest("Please enter a gamecode");
            }

            try
            {
                // create new dbcontext to get the game
                using (var db = new rock_paper_scissorsContext())
                {
                    //get the game from the database using the gamecode
                    var games = await _context.Games.FirstOrDefaultAsync(g => g.Gamecode == gamecode.gameCode);

                    //if the game is null then return bad request
                    if (games == null)
                    {
                        return BadRequest("Game does not exist");
                    }

                    var findRounds = await _context.Rounds.Where(r => r.Gameid == games.Gameid).ToListAsync();


                    if (findRounds == null)
                    {
                        return BadRequest("Game Winner has not been found");
                    }

                    //create a new list to store the rounds
                    var roundList = new List<GameResultResponse_RoundModel>();

                    //add finalrounds to roundlist
                    foreach (var round in findRounds)
                    {
                        roundList.Add(new GameResultResponse_RoundModel
                        {
                            RoundNumber = round.Roundnumber,
                            PlayerOneChoice = round.PlayerOneChoice,
                            PlayerTwoChoice = round.PlayerTwoChoice,
                            Winner = round.Winner
                        });
                    }

                    
                    //return the game result
                    return Ok(roundList);


                }
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
