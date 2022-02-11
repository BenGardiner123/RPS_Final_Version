#nullable disable
namespace RPS_Final_Version.Models.ViewModels
{
    public class GameResultResponseModel
    {
        public List<GameResultResponse_RoundModel> Rounds { get; set; }
        public string Winner { get; set; }
        
    }
}