using System.Collections;
using Microsoft.AspNetCore.Mvc;
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

            //get the most used choice per username
            var choiceCount = _context.Rounds.GroupBy(x => x.PlayerOneChoice).Select(x => new { Username = x.Key, Count = x.Count() }).ToList();
            
            int rockCount = 0;
            int paperCount = 0;
            int scissorsCount = 0;

            foreach(var choice in choiceCount){
                if(choice.Username == "Rock"){
                    rockCount = choice.Count;
                }
                if(choice.Username == "Paper"){
                    paperCount = choice.Count;
                }
                if(choice.Username == "Scissors"){
                    scissorsCount = choice.Count;
                }
            }


            //iterate over each of the lists and for each username add them to a leaderboard_player view model then add them to a leaderboard view model
            var leaderboard_player = new LeaderboardViewModel_Player();
            var leaderboard = new LeaderboardViewModel();

            //check if the list inside leaderboard is null - because i was getting a null reference exception
            //not sure on best pracice - i prolly should do this in the constructor
            if (leaderboard.leaders == null)
            {
                leaderboard.leaders = new List<LeaderboardViewModel_Player>();
            }   

            foreach(var game in gameCount){
                leaderboard_player.Username = game.Username;
                leaderboard_player.GamesPlayed = game.Count;
                leaderboard_player.GamesWon = gameWon.Find(x => x.Username == game.Username).Count;
                leaderboard_player.GamesLost = gameLost.Find(x => x.Username == game.Username).Count;
                leaderboard_player.GamesTied = gameTied.Find(x => x.Username == game.Username).Count;
                leaderboard_player.WinPercentage = (double)gameWon.Find(x => x.Username == game.Username).Count / (double)game.Count;
                leaderboard_player.MostUsedChoice = (rockCount > scissorsCount && rockCount > paperCount) ? "Rock" :
                                    (scissorsCount > rockCount && scissorsCount > paperCount) ? "Scissors" : "Paper";
                //add the leaderboard_player to the leaderboard

                leaderboard.leaders.Add(leaderboard_player);
            }



            return Ok(leaderboard);



        }


    }

}
