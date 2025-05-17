using CyberSecurityChatBot.Services;
using CyberSecurityChatBot.Utils;


namespace CyberSecurityChatBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Bot";
            Console.OutputEncoding = System.Text.Encoding.UTF8;//accepts only standard characters

            StartupService.PlayVoiceGreeting(); // Play welcome audio (from resources folder)
            StartupService.DisplayAsciiArt(); // Show startup banner

            Console.Write("Please enter your name: ");//name prompt for personalized greeting
            string userName = Console.ReadLine();
            Console.WriteLine();
            TextEffects.DisplayDivider();//dividers are easier on the eyes
            TextEffects.TypeEffect($"Hello, {userName}! I'm your Cybersecurity Awareness Bot.", 35); // Personalized welcome
            TextEffects.DisplayDivider();

            ChatBot.Start(userName); // Start chat loop
        }

    }
}







       