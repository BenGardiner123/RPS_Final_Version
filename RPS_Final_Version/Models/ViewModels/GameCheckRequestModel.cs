#nullable disable
namespace RPS_Final_Version.Models.ViewModels
{
    public class GameCheckRequestModel
    {
        public string Username { get; set; }
        public int roundLimit { get; set; }
        public DateTime DateTimeStarted { get; set; }
        public int roundCounter { get; set; }

    }
}
