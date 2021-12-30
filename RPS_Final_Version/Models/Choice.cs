using System;
using System.Collections.Generic;

namespace RPS_Final_Version.Models
{
    public partial class Choice
    {
        public Choice()
        {
            RoundPlayerOneChoiceNavigations = new HashSet<Round>();
            RoundPlayerTwoChoiceNavigations = new HashSet<Round>();
        }

        public string Description { get; set; } = null!;

        public virtual ICollection<Round> RoundPlayerOneChoiceNavigations { get; set; }
        public virtual ICollection<Round> RoundPlayerTwoChoiceNavigations { get; set; }
    }
}
