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
    }
}