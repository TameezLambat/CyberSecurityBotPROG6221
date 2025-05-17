using CyberSecurityChatBot.Services;
using CyberSecurityChatBot.Utils;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CyberSecurityChatBot
{
    public static class ChatBot
    {
        private static readonly Random _rng = new();

        //Variable advice strings
        private static readonly string[] PasswordAdvice =
        {
            //Set A for PasswordAdvice
            string.Join("\n", new[]
            {
            "1.) Use long, unique passwords for each site.",
            "   - A strong password should be at least 12 characters long and contain a mix of upper/lowercase letters, numbers, and symbols.",
            "   - Example: 'A3#r9lX7mQ' is far stronger than 'password123'.",
            "2.) Avoid using the same password for multiple sites.",
            "3.) Use a reputable password manager to store and generate strong passwords."
        }),
            //Set B for PasswordAdvice
            string.Join("\n",new[]
                {
             "🔑  Keep every password unique and 12+ chars.",
            "💡  Mix upper/lowercase letters, numbers, and symbols.",
            "🔒  One account hacked ≠ all accounts hacked.",
            "✅  A password manager remembers the complexity for you."
                })
                };



        private static readonly string[] PhishingAdvice =
            {
            //set A for Phising Advice
            string.Join("\n", new[]
        {
            "1.) Be cautious with urgent or unexpected emails.",
            "2.) Verify the sender’s email address carefully.",
            "3.) Never download attachments from unknown sources."
        }),
            //set B for Phising Advice
            string.Join("\n", new[]
        {
                "1.) Be cautious with urgent or unexpected emails.",
            "2.) Verify the sender’s email address carefully.",
            "3.) Never download attachments from unknown sources."
            })
        };

        private static readonly string[] BrowsingAdvice =
            {
            string.Join("\n", new[]
        {
            "1.) Always use HTTPS websites.",
            "2.) Avoid clicking pop‑ups and suspicious ads.",
            "3.) Use a VPN when browsing on public Wi‑Fi."
        }),
            //set B
            string.Join("\n", new[]
            {
                "1.) Always use HTTPS websites.",
            "2.) Avoid clicking pop‑ups and suspicious ads.",
            "3.) Use a VPN when browsing on public Wi‑Fi."
            })
        };


        // ---------- regex map ----------
        private static readonly Dictionary<string, string[]> RegexResponses = new()
        {
            { @"\b(password|credentials|login)\b",      PasswordAdvice },
            { @"\b(phishing|scam|fake email|fraud)\b",  PhishingAdvice },
            { @"\b(https|vpn|wifi|browsing|internet)\b", BrowsingAdvice },
            { @"\b(hello|hi|hey|yo)\b", new[] { "👋 Hello! Ask me about passwords, phishing, or safe browsing."} },
            { @"\b(how are you|how's it going|how do you do)\b", new[] { "😊 I'm running smoothly and ready to help keep you safe online!" } },
            { @"\b(who are you|what are you|purpose)\b", new[] {"🤖 I'm a Cybersecurity Awareness Bot designed to share safety tips." } }
        };

        // ---------- minimal topic menu ----------
        private const string MenuText =
            "\n1. Password Safety\n" +
            "2. Phishing & Scams\n" +
            "3. Safe Browsing\n" +
            "Type 1‑3, or just ask naturally. Type 'help' to see this menu again.\n";

        // ---------- public entry ----------
        public static void Start(string userName)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(MenuText);
            Console.ResetColor();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{userName} › ");
                Console.ResetColor();

                string input = Console.ReadLine()?.ToLower().Trim();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("⚠️  Please say something.");
                    continue;
                }

                if (input is "exit" or "quit")
                {
                    Console.WriteLine("👋 Stay safe out there!");
                    break;
                }

                if (input is "menu" or "help")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(MenuText);
                    Console.ResetColor();
                    continue;
                }

                // numeric shortcuts 1‑3
                if (int.TryParse(input, out int n))
                {
                    input = n switch
                    {
                        1 => "password",
                        2 => "phishing",
                        3 => "https",
                        _ => input
                    };
                }

                bool matched = false;
                foreach (var kvp in RegexResponses)
                {
                    if (Regex.IsMatch(input, kvp.Key))
                    {
                        string response = kvp.Value[_rng.Next(kvp.Value.Length)];
                        TextEffects.TypeEffect(response, 10);



                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    TextEffects.TypeEffect("🤔 I’m not sure about that. Try passwords, phishing, or safe browsing tips.");
                }
            }
        }
    }
}

