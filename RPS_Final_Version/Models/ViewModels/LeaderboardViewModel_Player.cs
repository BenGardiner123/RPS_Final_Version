#nullable disable
namespace RPS_Final_Version.Models.ViewModels
{
    public class LeaderboardViewModel_Player
    {
        public string Username { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int GamesLost { get; set; }
        public int GamesTied { get; set; }
        public double WinPercentage { get; set; }
        public string MostUsedChoice { get; set; }
    }
}
