using System;
using System.Collections.Generic;

namespace RPS_Final_Version.Models
{
    public partial class Round
    {
        public int Roundnumber { get; set; }
        public int Gameid { get; set; }
        public string PlayerOneChoice { get; set; } = null!;
        public string PlayerTwoChoice { get; set; } = null!;
        public string Winner { get; set; } = null!;

        public virtual Game Game { get; set; } = null!;
        public virtual Choice PlayerOneChoiceNavigation { get; set; } = null!;
        public virtual Choice PlayerTwoChoiceNavigation { get; set; } = null!;
    }
}
