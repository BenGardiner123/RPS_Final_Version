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
        public IEnumerable<LeaderboardViewModel> GetLeaderboard()
        {
            
            //get all the players thern calualte games played, games won gmaes lsot and games tied, win percentage and most used choice and then add them to the leaderboard view model
            var players = _context.Players.ToList();
            var leaderboard = new List<LeaderboardViewModel_Player>();
            foreach (var player in players)
            {
                var listOfGamesIds = _context.Games.Where(g => g.PlayerOne == player.Username).Select(g => g.Gameid).ToList();
                var gamesPlayed = _context.Games.Count();
                
                //get the rounds played where the playerid is matching with game id in rounds
                var roundsPlayed = _context.Rounds.Intersect(_context.Rounds.Where(r => r.Gameid == gamesPlayed.)).Count();
                //get the games won
                var gamesWon = _context.Games.Where(g => g.GameWinner == player.Username).Count();

                //get the games lost
                var gamesLost = _context.Games.Where(g => g.GameWinner != player.Username).Count();

                //get the games tied
                var gamesTied = _context.Games.Where(g => g.GameWinner == "Draw").Count();

                //calculate the win percentage
                var winPercentage = (double)gamesWon / (double)gamesPlayed;

                //get the rounds for each player 
                
                var leader = new LeaderboardViewModel_Player
                {
                    Username = player.Username,
                    GamesPlayed = gamesPlayed,
                    GamesWon = gamesWon,
                    GamesLost = gamesLost,
                    GamesTied = gamesTied,
                    WinPercentage = winPercentage,
                    MostUsedChoice = mostUsedChoice
                };
                leaderboard.Add(leader);
            }

        }







    }
}
