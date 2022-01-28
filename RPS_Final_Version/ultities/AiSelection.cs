namespace RPS_Final_Version.ultities
{
    public class AiSelection
    {
        //create a method that returns a random choice of "Rock", "Paper", or "Scissors"
        public string AiChoice()
        {
            //create a random number between 1 and 3
            Random random = new Random();
            int randomNumber = random.Next(1, 4);

            //if the random number is 1 then return "Rock"
            if (randomNumber == 1)
            {
                return "Rock";
            }
            //if the random number is 2 then return "Paper"
            else if (randomNumber == 2)
            {
                return "Paper";
            }
            //if the random number is 3 then return "Scissors"
            else
            {
                return "Scissors";
            }
        }   

        //calulate the winner  of the rock paper scissors
        public string CalculateGameWinner(string playerOneChoice, string playerTwoChoice){
            //if the player one choice is "Rock" and the player two choice is "Scissors" then player one wins
            if (playerOneChoice == "Rock" && playerTwoChoice == "Scissors")
            {
                return "Player One";
            }
            //if the player one choice is "Rock" and the player two choice is "Paper" then player two wins
            else if (playerOneChoice == "Rock" && playerTwoChoice == "Paper")
            {
                return "Player Two";
            }
            //if the player one choice is "Paper" and the player two choice is "Rock" then player one wins
            else if (playerOneChoice == "Paper" && playerTwoChoice == "Rock")
            {
                return "Player One";
            }
            //if the player one choice is "Paper" and the player two choice is "Scissors" then player two wins
            else if (playerOneChoice == "Paper" && playerTwoChoice == "Scissors")
            {
                return "Player Two";
            }
            //if the player one choice is "Scissors" and the player two choice is "Rock" then player two wins
            else if (playerOneChoice == "Scissors" && playerTwoChoice == "Rock")
            {
                return "Player Two";
            }
            //if the player one choice is "Scissors" and the player two choice is "Paper" then player one wins
            else if (playerOneChoice == "Scissors" && playerTwoChoice == "Paper")
            {
                return "Player One";
            }
            //if the player one choice is the same as the player two choice then it is a tie
            else if (playerOneChoice == playerTwoChoice)
            {
                return "Tie";
            }
            //if the player one choice is not a valid choice then return an error
            else
            {
                return "Error";
            }

        }
    }

    
    
}