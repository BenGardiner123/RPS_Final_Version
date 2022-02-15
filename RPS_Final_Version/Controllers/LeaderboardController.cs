using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPS_Final_Version.Models;
using RPS_Final_Version.Models.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RPS_Final_Version.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        ///swagger endpoint because i always forget to add it
        /// https://localhost:7066/swagger/index.html
        private readonly rock_paper_scissorsContext _context;
        public IConfiguration Configuration { get; }

        public LeaderboardController(rock_paper_scissorsContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;

        }

        // GET: api/<Leaderboard>
        [HttpGet("Leaderboard")]
        public ActionResult<LeaderboardViewModel> GetLeaderboard()
        {

            //get all the games in the database
            var games = _context.Games.AsEnumerable();

            
            
            //get the count from games played per username
            var gameCount = _context.Games.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Count = x.Count() }).ToList();

            //get the games won per username
            var gameWon = _context.Games.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Count = x.Count(y => y.GameWinner == "Player One") }).ToList();

            //get the games lost per username
            var gameLost = _context.Games.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Count = x.Count(y => y.GameWinner == "Player Two") }).ToList();

            //get the games tied per username
            var gameTied = _context.Games.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Count = x.Count(y => y.GameWinner == "Draw") }).ToList();

            //get the win percentage per username
            var winPercentage = _context.Games.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Count = x.Count(y => y.GameWinner == "Player One") / x.Count() }).ToList();

            //join the game and round on game id then get the playeronechoice where equal to player one in the gtame table
            var playerOneChoice = _context.Games.Join(_context.Rounds, g => g.Gameid, r => r.Gameid, (g, r) => new { g.PlayerOne, r.PlayerOneChoice }).ToList();

            var check = playerOneChoice;

            //for each player one in playeronechoice get the most commonly occuring PlayerOneChoice
            var playerOneChoiceList = playerOneChoice.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Choice = x.GroupBy(y => y.PlayerOneChoice).Select(y => y.Count()).OrderByDescending(y => y).FirstOrDefault(),  }).ToList();

            var checker = playerOneChoiceList;

            //for each player one in playeronechoice get the most commonly occuring PlayerOneChoice
            var rockCount = playerOneChoice.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Choice = x.GroupBy(y => y.PlayerOneChoice).Select(y => y.Count(z => z.PlayerOneChoice == "Rock")).OrderByDescending(y => y).FirstOrDefault(), }).ToList();
            var paperCount = playerOneChoice.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Choice = x.GroupBy(y => y.PlayerOneChoice).Select(y => y.Count(z => z.PlayerOneChoice == "Paper")).OrderByDescending(y => y).FirstOrDefault(), }).ToList();
            var scissorsCount = playerOneChoice.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Choice = x.GroupBy(y => y.PlayerOneChoice).Select(y => y.Count(z => z.PlayerOneChoice == "Scissors")).OrderByDescending(y => y).FirstOrDefault(), }).ToList();

            // for each playerone in rockcount get the most commonly occuring PlayerOneChoice
            var rockList = rockCount.GroupBy(x => x.Username).Select(x => new { Username = x.Key, Choice = x.GroupBy(y => y.Choice).Select(y => y.Count()).OrderByDescending(y => y).FirstOrDefault(), }).ToList();
            var paperList = paperCount.GroupBy(x => x.Username).Select(x => new { Username = x.Key, Choice = x.GroupBy(y => y.Choice).Select(y => y.Count()).OrderByDescending(y => y).FirstOrDefault(), }).ToList();
            var scissorsList = scissorsCount.GroupBy(x => x.Username).Select(x => new { Username = x.Key, Choice = x.GroupBy(y => y.Choice).Select(y => y.Count()).OrderByDescending(y => y).FirstOrDefault(), }).ToList();

            /// need to find the highest countr per username between rockcount , papercount and scissorscount
            /// then get the username from the rockcount where the count is the highest
        

            //create leaderboard view model
            var leaderboard = new LeaderboardViewModel();

            
            //check if the list inside leaderboard is null - because i was getting a null reference exception
            //not sure on best pracice - i prolly should do this in the constructor
            if (leaderboard.leaders == null)
            {
                leaderboard.leaders = new List<LeaderboardViewModel_Player>();
            }   

            foreach(var game in gameCount){
                //create a new leaderboard view model player
                var leaderboardPlayer = new LeaderboardViewModel_Player();
                leaderboardPlayer.Username = game.Username;
                leaderboardPlayer.GamesPlayed = game.Count;
                leaderboardPlayer.GamesWon = gameWon.Find(x => x.Username == game.Username).Count;
                leaderboardPlayer.GamesLost = gameLost.Find(x => x.Username == game.Username).Count;
                leaderboardPlayer.GamesTied = gameTied.Find(x => x.Username == game.Username).Count;
                leaderboardPlayer.WinPercentage = (double)gameWon.Find(x => x.Username == game.Username).Count / (double)game.Count;
                
                
                
                leaderboard.leaders.Add(leaderboardPlayer);
            }



            return Ok(leaderboard);

        }
            

        


    }

}
