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
        [HttpGet("getLeaderboard")]
        public IEnumerable GetLeaderboard()
        {
            //for ecah game get the player username and their most used choice for each round
            // var leaderboard = (from g in _context.Games
            //                    join p in _context.Players on g.PlayerOne equals p.Username
            //                    join r in _context.Rounds on g.Gameid equals r.Gameid
            //                    join c in _context.Choices on r.PlayerOneChoice equals c.Description
            //                    group new { p, r, c } by new { p.Username } into g
            //                    select new LeaderboardViewModel_Player
            //                    {
            //                        Username = g.Key.Username,
            //                        //games played where the username == player one in games
            //                        GamesPlayed = g.Count(x => x.p.Username == g.Key.Username),
            //                        //games won is the count of the number of times the player won
            //                        GamesWon = g.Count(x => x.r.Winner == "Player One"),
            //                        //games lost is the count of the number of times the player lost
            //                        GamesLost = g.Count(x => x.r.Winner == "Player Two"),
            //                        //games tied is the count of the number of times the player tied
            //                        GamesTied = g.Count(x => x.r.Winner == "Draw"),
            //                        //win percentage is the percentage of games won divided by the total number of games played
            //                        WinPercentage = (double)g.Count(x => x.r.Winner == "Player One") / g.Count(),
            //                        //most used choice is the choice that was used the most in the game
            //                        MostUsedChoice = g.Max(x => x.c.Description)
            //                     }).ToList();

            

            // return Task.FromResult<ActionResult<LeaderboardViewModel>>(Ok(leaderboard));

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
            var RockOccurs = _context.Rounds.GroupBy(x => x.PlayerOneChoice).Select(x => new { Username = x.Key, Count = x.Count() }).ToList();
            //create a new list to hold the rock occurances
            var PaperOccurs = _context.Rounds.GroupBy(x => x.PlayerOneChoice).Select(x => new { Username = x.Key, Count = x.Count() }).ToList();
            var ScissorsOccurs = _context.Rounds.GroupBy(x => x.PlayerOneChoice).Select(x => new { Username = x.Key, Count = x.Count() }).ToList();

            var rockCount = RockOccurs.Count();
            var paperCount = PaperOccurs.Count();
            var scissorsCount = ScissorsOccurs.Count(); 

            int d = Math.Max(rockCount, Math.Max(paperCount, scissorsCount));

           


            //combine the data into one list
            var leaderboard = (from g in gameCount
                               join w in gameWon on g.Username equals w.Username
                               join l in gameLost on g.Username equals l.Username
                               join t in gameTied on g.Username equals t.Username
                               join p in winPercentage on g.Username equals p.Username
                               join r in RockOccurs on g.Username equals r.Username
                               join s in ScissorsOccurs on g.Username equals s.Username
                               join p2 in PaperOccurs on g.Username equals p2.Username
                               select new LeaderboardViewModel_Player
                               {
                                   Username = g.Username,
                                   GamesPlayed = g.Count,
                                   GamesWon = w.Count,
                                   GamesLost = l.Count,
                                   GamesTied = t.Count,
                                   WinPercentage = p.Count,
                                  
                               }).ToList();

            //create the leaderboard view model and return it
            

            return leaderboard;
       
            

        }
           

    }

}
