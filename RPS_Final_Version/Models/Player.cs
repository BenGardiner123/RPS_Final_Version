using System;
using System.Collections.Generic;

namespace RPS_Final_Version.Models
{
    public partial class Player
    {
        public Player()
        {
            GamePlayerOneNavigations = new HashSet<Game>();
            GamePlayerTwoNavigations = new HashSet<Game>();
        }

        public string Username { get; set; } = null!;

        public virtual ICollection<Game> GamePlayerOneNavigations { get; set; }
        public virtual ICollection<Game> GamePlayerTwoNavigations { get; set; }
    }
}
