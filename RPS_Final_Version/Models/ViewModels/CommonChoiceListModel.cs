#nullable disable

namespace RPS_Final_Version.Models.ViewModels
{
    public class CommonChoiceListModel
    {
       //create a rock list and fill it with the db choices
        public List<int> RockList { get; set; }
        public List<int> PaperList { get; set; }
        public List<int> ScissorsList { get; set; }

    }
}