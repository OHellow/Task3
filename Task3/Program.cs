using System;
using System.Text;
using System.Security.Cryptography;
using System.Linq;

namespace Task3
{
    class Program
    {
        static void Main(string[] gameOptions)
        {
            string playerChoice;
            string computerChoice;

            string key = keyGenerator();

            bool isFinished = false;
            while (!isFinished)
            {
                Console.WriteLine("HMAC:\n" + makeHMAC(key));
                showOptions(gameOptions);
                if (gameOptions.Length < 3 || gameOptions.Length % 2 == 0 || !isInputCorrect(gameOptions))
                {
                    Console.WriteLine("There are must be 3 arguments at least and it's number must be odd\n" +
                    "For example: Rock Paper Scissors\nor\n Rock Paper Scissors Lizard Spock\nPlease, try again:");
                    gameOptions = Console.ReadLine().Split(' ');
                }
                else
                {
                    Console.WriteLine("Enter your move: ");
                    playerChoice = Console.ReadLine();
                    while (!isFinished)
                    {
                        if (isMoveCorrect(gameOptions, playerChoice))
                        {
                            computerChoice = compChoice(gameOptions);
                            showChoises(computerChoice, playerChoice, gameOptions);
                            showWinner(gameOptions, computerChoice, playerChoice);
                            Console.WriteLine("\nHMAC: " + makeHMAC(key));
                            isFinished = true;
                        }
                        else if (playerChoice == "0")
                        {
                            Console.WriteLine("Thank you for game!");
                            isFinished = true;
                        }
                        else
                        {
                            Console.Write("Please enter correct option:");
                            playerChoice = Console.ReadLine();
                        }
                    }
                }
            }
        }

        public static bool isInputCorrect(string[] playerChoiceArray)
        {
            return playerChoiceArray.Distinct().ToArray().Length == playerChoiceArray.Length;
        }

        public static bool isMoveCorrect(string[] playerChoiceArray, string move)
        {
            return Convert.ToInt32(move, 10) >= 1 && Convert.ToInt32(move, 10) <= playerChoiceArray.Length;
        }

        public static string makeHMAC(string key)
        {
            StringBuilder hmacKey = new StringBuilder();
            foreach (byte element in SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key.ToString())))
            {
                hmacKey.Append(element.ToString("x2"));
            }
            return hmacKey.ToString();
        }

        public static string keyGenerator()
        {
            byte[] randomThing = new byte[16];
            RandomNumberGenerator.Create().GetBytes(randomThing);
            StringBuilder newKey = new StringBuilder();
            foreach (byte element in randomThing)
            {
                newKey.Append(element.ToString("x2"));
            }
            return newKey.ToString();
        }

        public static void showOptions(string[] options)
        {
            int index = 1;
            foreach (string option in options)
            {
                Console.WriteLine(index + " - " + option);
                index++;
            }
            Console.WriteLine("0 - Exit");
        }

        public static string compChoice(string[] computerChoiceArray)
        {
            int compChoise = new Random().Next(1, computerChoiceArray.Length + 1);
            return Convert.ToString(compChoise);
        }

        public static void showChoises(string compOption, string playerOption, string[] optionsArray)
        {
            string compMove = optionsArray[Int32.Parse(compOption) - 1];
            string playerMove = optionsArray[Int32.Parse(playerOption) - 1];
            Console.WriteLine("Computer move: " + compMove + "\nPlayer move: " + playerMove);
        }

        public static void showWinner(string[] playerOptionsArray, string computerChoice, string playerChoice)
        {
            int computerMove = Convert.ToInt32(computerChoice, 10);
            int playerMove = Convert.ToInt32(playerChoice, 10);
            bool result = (Math.Abs(computerMove - playerMove)) <= (playerOptionsArray.Length / 2);
            if (computerChoice == playerChoice)
            {
                Console.WriteLine("Draw!");
            }
            else if ((result && computerMove > playerMove)|| (!result && computerMove < playerMove))
            {
                Console.WriteLine("Computer wins!");
            }
            else
            {
                Console.WriteLine("You win!");
            }
        }
    }
}
