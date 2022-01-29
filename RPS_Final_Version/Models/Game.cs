using System;
using System.Collections.Generic;

#nullable disable

namespace RPS_Final_Version.Models
{
    public partial class Game
    {
        public Game()
        {
            Rounds = new HashSet<Round>();
        }

        public int Gameid { get; set; }
        public string Gamecode { get; set; } = null!;
        public string GamerWinner { get; set; }
        public int Roundlimit { get; set; }
        public DateTime Datetimestarted { get; set; }
        public DateTime Datetimeended { get; set; }
        public string PlayerOne { get; set; } = null!;
        public string PlayerTwo { get; set; } = null!;

        public virtual Player PlayerOneNavigation { get; set; } = null!;
        public virtual Player PlayerTwoNavigation { get; set; } = null!;
        public virtual ICollection<Round> Rounds { get; set; }
    }
}
