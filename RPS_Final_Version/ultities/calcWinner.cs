using RPS_Final_Version.Models;

namespace RPS_Final_Version.ultities
{
    public class calcWinner
    {
        public string CalulateGameWinner(List<Round> roundList)
        {
            //create varibles for palyer one, player two and draw
            var playerOneWins = 0;
            var playerTwoWins = 0;
            var draw = 0;
            var actualWinner = "";

            //loop through the rounds and check who won
            foreach (var roundItem in roundList)
            {
                if (roundItem.Winner == "Player One")
                {
                    playerOneWins++;
                }
                else if (roundItem.Winner == "Player Two")
                {
                    playerTwoWins++;
                }
                else if (roundItem.Winner == "Draw")
                {
                    draw++;
                }
            }



            //check if player one won
            if (playerOneWins > playerTwoWins)
            {
                actualWinner = "Player One";
            }
            else if (playerTwoWins > playerOneWins)
            {
                actualWinner = "Player Two";
            }
            else if (playerOneWins == playerTwoWins)
            {
                actualWinner = "Draw";
            }

            //update the Game table with the winner 
            return actualWinner;
        }
        
    }
}