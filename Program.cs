using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CybersecurityBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Bot";

            // ========================
            // 🎨 ASCII Art Logo
            // ========================
            DisplayAsciiArt();

            // ========================
            // 🙋 Name Input
            // ========================
            Console.Write("Please enter your name: ");
            string userName = Console.ReadLine();
            Console.WriteLine();
            DisplayDivider();
            TypeEffect($"Hello, {userName}! I'm your Cybersecurity Awareness Bot.", 35);
            DisplayDivider();

            // ========================
            // 💬 Chat Loop
            // ========================
            ChatLoop(userName);
        }

        static void DisplayAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
   ____            _                 _                 
  / ___|___  _ __ | |_ _   _ _ __   | | ___   ___  ___ 
 | |   / _ \| '_ \| __| | | | '_ \  | |/ _ \ / _ \/ __|
 | |__| (_) | | | | |_| |_| | |_) | | | (_) | (_) \__ \
  \____\___/|_| |_|\__|\__,_| .__/  |_|\___/ \___/|___/
                            |_|                        
            ");
            Console.ResetColor();
        }

        static void ChatLoop(string userName)
        {
            // Predefined topics and associated keywords/responses
            var topicResponses = new Dictionary<string[], string>
            {
                { new[] { "hello", "hi", "hey", "how are you", "sup", "what's up" },
                    "I'm great, thanks! I'm here to help you stay safe online." },

                { new[] { "purpose", "what are you", "why do you exist" },
                    "My purpose is to raise awareness about cybersecurity best practices." },

                { new[] { "password", "passwords", "strong password", "secure password" },
                    "✅ Use long, unique passwords and enable two-factor authentication." },

                { new[] { "phishing", "scam email", "fake email", "suspicious link" },
                    "🚨 Don’t click suspicious links or provide personal info to unknown sources." },

                { new[] { "safe browsing", "browsing", "internet safety", "browse safely" },
                    "🛡️ Keep your browser up to date and avoid sketchy websites." },

                { new[] { "what can i ask", "help", "topics", "options" },
                    "You can ask me about passwords, phishing, safe browsing, and general online safety." }
            };

            while (true)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{userName}, ask me something: ");
                Console.ResetColor();

                string input = Console.ReadLine()?.ToLower().Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("⚠️ Please enter a valid question.");
                    Console.ResetColor();
                    continue;
                }

                DisplayDivider();

                bool matched = false;

                foreach (var entry in topicResponses)
                {
                    if (entry.Key.Any(keyword => input.Contains(keyword)))
                    {
                        TypeEffect(entry.Value);
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    TypeEffect("Hmm... I didn’t quite get that. Can you rephrase?");
                }

                DisplayDivider();
            }
        }

        // ========== UI Helpers ==========

        static void TypeEffect(string text, int delay = 25)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        static void DisplayDivider()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('-', 60));
            Console.ResetColor();
        }
    }
}
