#nullable disable
namespace RPS_Final_Version.Models.ViewModels
{
    public class GameResultResponseModel
    {
        public List<Round> Rounds { get; set; }
        public string GameWinner { get; set; }
    }
}