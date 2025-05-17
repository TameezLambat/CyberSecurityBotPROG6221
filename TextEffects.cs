using System;
using System.Threading;

namespace CyberSecurityChatBot.Utils
{
    public static class TextEffects
    {
        public static void TypeEffect(string message, int delay = 20)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        public static void DisplayDivider(int width = 55)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('-', width));
            Console.ResetColor();
        }
    }
}
