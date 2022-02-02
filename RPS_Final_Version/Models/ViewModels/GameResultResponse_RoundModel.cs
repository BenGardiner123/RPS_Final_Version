namespace RPS_Final_Version.Models.ViewModels
{
    public class GameResultResponse_RoundModel
    {
        public int RoundNumber { get; set; }
        public string PlayerOneChoice { get; set; } = null!;
        public string PlayerTwoChoice { get; set; } = null!;
        public string Winner { get; set; } = null!;
    }
}