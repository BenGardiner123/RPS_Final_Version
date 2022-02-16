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
            var games = _context.Games;

            //get the count from games played per username
            var gameCount = games.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Count = x.Count() }).ToList();

            //get the games won per username
            var gameWon = games.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Count = x.Count(y => y.GameWinner == "Player One") }).ToList();

            //get the games lost per username
            var gameLost = games.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Count = x.Count(y => y.GameWinner == "Player Two") }).ToList();

            //get the games tied per username
            var gameTied = games.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Count = x.Count(y => y.GameWinner == "Draw") }).ToList();

            //get the win percentage per username
            var winPercentage = games.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Count = x.Count(y => y.GameWinner == "Player One") / x.Count() }).ToList();

            //join the game and round on game id then get the playeronechoice where equal to player one in the gtame table
            var playerOneChoice = _context.Games.Join(_context.Rounds, g => g.Gameid, r => r.Gameid, (g, r) => new { g.PlayerOne, r.PlayerOneChoice }).ToList();

            //for each player one in playeronechoice get the most commonly occuring PlayerOneChoice
            var rockCount = playerOneChoice.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Choice = x.GroupBy(y => y.PlayerOneChoice).Select(y => y.Count(z => z.PlayerOneChoice == "Rock")).OrderByDescending(y => y).FirstOrDefault(), }).ToList();
            var paperCount = playerOneChoice.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Choice = x.GroupBy(y => y.PlayerOneChoice).Select(y => y.Count(z => z.PlayerOneChoice == "Paper")).OrderByDescending(y => y).FirstOrDefault(), }).ToList();
            var scissorsCount = playerOneChoice.GroupBy(x => x.PlayerOne).Select(x => new { Username = x.Key, Choice = x.GroupBy(y => y.PlayerOneChoice).Select(y => y.Count(z => z.PlayerOneChoice == "Scissors")).OrderByDescending(y => y).FirstOrDefault(), }).ToList();

    
            //create leaderboard view model
            var leaderboard = new LeaderboardViewModel();


            //check if the list inside leaderboard is null - because i was getting a null reference exception
            //not sure on best pracice - i prolly should do this in the constructor
            if (leaderboard.leaders == null)
            {
                leaderboard.leaders = new List<LeaderboardViewModel_Player>();
            }

            foreach (var game in gameCount)
            {
                //create a new leaderboard view model player
                var leaderboardPlayer = new LeaderboardViewModel_Player();
                leaderboardPlayer.Username = game.Username;
                leaderboardPlayer.GamesPlayed = game.Count;
                leaderboardPlayer.GamesWon = gameWon.Find(x => x.Username == game.Username).Count;
                leaderboardPlayer.GamesLost = gameLost.Find(x => x.Username == game.Username).Count;
                leaderboardPlayer.GamesTied = gameTied.Find(x => x.Username == game.Username).Count;
                leaderboardPlayer.WinPercentage = (double)gameWon.Find(x => x.Username == game.Username).Count / (double)game.Count;
                var rockCounter = 0;
                var paperCounter = 0;
                var scissorsCounter = 0;
                //get the count from RockCount where username is equal to the username in the game count
                foreach (var rock in rockCount)
                {
                    if (rock.Username == game.Username)
                    {
                        rockCounter = rock.Choice;
                    }
                }
                //get the count from PaperCount where username is equal to the username in the game count
                foreach (var paper in paperCount)
                {
                    if (paper.Username == game.Username)
                    {
                        paperCounter = paper.Choice;
                    }
                }
                //get the count from ScissorsCount where username is equal to the username in the game count
                foreach (var scissors in scissorsCount)
                {
                    if (scissors.Username == game.Username)
                    {
                        scissorsCounter = scissors.Choice;
                    }
                }

                if(rockCounter > paperCounter && rockCounter > scissorsCounter)
                {
                    leaderboardPlayer.MostUsedChoice = "Rock";
                }
                else if(paperCounter > rockCounter && paperCounter > scissorsCounter)
                {
                    leaderboardPlayer.MostUsedChoice = "Paper";
                }
                else if(scissorsCounter > rockCounter && scissorsCounter > paperCounter)
                {
                    leaderboardPlayer.MostUsedChoice = "Scissors";
                }
                

               
                leaderboard.leaders.Add(leaderboardPlayer);
            }



            return Ok(leaderboard);

        }





    }

}
