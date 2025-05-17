using System;
using System.Media;

namespace CyberSecurityChatBot.Services
{
    public static class StartupService
    {
        public static void PlayVoiceGreeting()
        {
            try
            {
                var player = new SoundPlayer(@"Resources\\ttsmaker-file-2025-4-19-1-24-16.wav");
                player.PlaySync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ Could not play audio: " + ex.Message);
            }
        }

        public static void DisplayAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
╔════════════════════════════════════════════════════╗
║   YOUR PASSWORD SUCKS, BUT THAT'S FINE             ║
║   I'm your Cybersecurity Wingman                   ║
╚════════════════════════════════════════════════════╝
");
            Console.ResetColor();
        }

        public static void DisplayDivider()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('-', 55));
            Console.ResetColor();
        }
    }
}
