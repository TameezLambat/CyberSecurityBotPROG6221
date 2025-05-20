using CyberSecurityChatBot.Data;
using CyberSecurityChatBot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CyberSecurityChatBot
{
    public static class ChatBot
    {
        private static readonly Random _rng = new();

       
        //follow up
        private static readonly string[] FollowUps =
        {
            "tell me more", "what else", "why", "how", "expand", "can you explain", "more", "continue", "that’s confusing"
        };
        //clear memory. 
        private static readonly string[] ResetPhrases =
        {
            "new topic", "change topic", "switch", "let's talk about something else", "different question"
        };

        // Memory fields
        private static string? _userNameMemory = null;
        private static string? _currentTopic = null;
        private static string? _userInterest = null;

        public static void Start(string userName)
        {
            _userNameMemory = userName;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(ChatBotData.MenuText);
            Console.ResetColor();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{userName} › ");
                Console.ResetColor();

                string input = Console.ReadLine()?.ToLower().Trim() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("⚠️ Please say something.");
                    continue;
                }

                if (InputValidator.IsQuitCommand(input))
                {
                    Console.WriteLine("👋 Stay safe out there!");
                    break;
                }

                if (InputValidator.IsHelpCommand(input))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(ChatBotData.MenuText);
                    Console.ResetColor();
                    continue;
                }

                if (InputValidator.TryGetMenuChoice(input, out int n))
                {
                    input = n switch
                    {
                        1 => "password",
                        2 => "phishing",
                        3 => "https",
                        4 => "vpn",
                        5 => "privacy",
                        _ => input
                    };
                }
                else if (int.TryParse(input, out _))
                {
                    Console.WriteLine("❌ That number doesn’t match any topic. Please choose 1-5.");
                    continue;
                }

                if (InputValidator.IsGibberish(input))
                {
                    Console.WriteLine();
                    Console.WriteLine("🤖 That doesn’t look like a question—try asking about passwords, phishing, safe browsing, VPNs, or privacy.");
                    Console.WriteLine();
                    continue;
                }

                // Reset current topic if reset phrase is detected
                if (ResetPhrases.Any(p => input.Contains(p)))
                {
                    _currentTopic = null;
                    Console.WriteLine();
                    Console.WriteLine("🔄 Got it! Let’s start a new topic.");
                    Console.WriteLine();
                    continue;
                }

                bool matched = false;

                // Check if user is asking a follow-up
                bool isFollowUp = FollowUps.Any(phrase => input.Contains(phrase)) && _currentTopic != null;

                if (isFollowUp)
                {
                    foreach (var kvp in ChatBotData.RegexResponses)
                    {
                        if (kvp.Key == _currentTopic)
                        {
                            string followUpResponse = kvp.Value[_rng.Next(kvp.Value.Length)];
                            PrintBotResponse(followUpResponse);
                            matched = true;
                            break;
                        }
                    }

                    if (matched) continue;
                }

                // Sentiment detection
                string? sentimentMessage = null;
                string? topicResponse = null;

                foreach (var kvp in ChatBotData.SentimentKeywords)
                {
                    if (input.Contains(kvp.Key))
                    {
                        sentimentMessage = kvp.Value;
                        break;
                    }
                }

                // Topic detection
                foreach (var kvp in ChatBotData.RegexResponses)
                {
                    if (Regex.IsMatch(input, kvp.Key))
                    {
                        _currentTopic = kvp.Key;
                        _userInterest = _currentTopic; // store interest
                        topicResponse = kvp.Value[_rng.Next(kvp.Value.Length)];
                        break;
                    }
                }

                if (sentimentMessage != null || topicResponse != null)
                {
                    Console.WriteLine();

                    if (sentimentMessage != null)
                        TextEffects.TypeEffect($"I understand that {_userNameMemory}, {sentimentMessage}", 15);//prints users name and sentiment statement

                    if (topicResponse != null)
                    {
                        if (_userInterest != null && sentimentMessage == null)
                        {
                            Console.WriteLine($"🤖 Since you're interested in this, here's something useful:");
                        }
                        PrintBotResponse(topicResponse);
                    }

                    continue;
                }

                // Fallback response
                if (!matched)
                {
                    if (_userInterest != null)
                    {
                        Console.WriteLine($"🤔 You’ve previously shown interest in {_userInterest.Replace(@"\b", "")}. Would you like to revisit that?");
                    }
                    else
                    {
                        PrintBotResponse("🤖 I'm not quite sure how to respond to that. Try asking about passwords, phishing, VPNs, or privacy.");
                    }
                }
            }
        }

        private static void PrintBotResponse(string message)
        {
            Console.WriteLine(); // Blank line before bot reply

            string[] lines = message.Split('\n');
            foreach (string line in lines)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Bot 🤖 › {line}");
            }

            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
