using CyberSecurityChatBot.Data;
using CyberSecurityChatBot.Utils;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace CyberSecurityChatBot
{
    public static class ChatBot
    {
        private static readonly Dictionary<string, int> _topicMentionCount = new();

        private static readonly Random _rng = new();
        private static readonly string[] FollowUps =

        {
            "tell me more","elaborate", "what else", "why", "how", "expand", "can you explain", "more", "continue", "that’s confusing"
        };

        private static readonly string[] InterestLeadIns =
        {
            "Since you're interested in {0}, here’s something useful:",
            "Here’s something helpful related to {0}:",
            "Let’s explore more about {0}:",
            "This might interest you as someone who cares about {0}:"
        };

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
                    ShowMenu();
                    continue;
                }

                if (InputValidator.TryGetMenuChoice(input, out int n))
                {
                    input = MapMenuChoiceToTopic(n);
                }
                else if (int.TryParse(input, out _))
                {
                    Console.WriteLine("❌ That number doesn’t match any topic. Please choose 1–5.");
                    continue;
                }

                if (InputValidator.IsGibberish(input))
                {
                    HandleGibberish();
                    continue;
                }

                if (HandleFollowUp(input))
                    continue;

                string? sentimentMessage = DetectSentiment(input);
                string? topicResponse = DetectTopic(input);

                if (sentimentMessage != null || topicResponse != null)
                {
                    RespondToInput (sentimentMessage, topicResponse);
                    continue;
                }

                HandleFallback();
            }
        }

        private static void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(ChatBotData.MenuText);
            Console.ResetColor();
        }

        private static string MapMenuChoiceToTopic(int choice) => choice switch
        {
            1 => "password",
            2 => "phishing",
            3 => "https",
            4 => "vpn",
            5 => "privacy",
            _ => ""
        };

        private static void HandleGibberish()
        {
            Console.WriteLine();
            Console.WriteLine("🤖 That doesn’t look like a question, try asking about passwords, phishing, safe browsing, VPNs, or privacy.");
            Console.WriteLine();
        }

        private static bool HandleFollowUp(string input)
        {
            bool isFollowUp = FollowUps.Any(phrase => input.Contains(phrase)) && _currentTopic != null;
            if (!isFollowUp) return false;

            if (_currentTopic != null)
            {
                // Increment count for current topic during follow-up for favourite topic
                if (!_topicMentionCount.ContainsKey(_currentTopic))
                    _topicMentionCount[_currentTopic] = 1;
                else
                    _topicMentionCount[_currentTopic]++;
            }

            if (ChatBotData.RegexResponses.TryGetValue(_currentTopic, out string[] responses))
            {
                // Show special third-time message if applicable
                if (_userInterest == _currentTopic &&
                    _topicMentionCount.TryGetValue(_currentTopic, out int count) &&
                    count == 3 &&
                    ChatBotData.PersistentInterestResponses.TryGetValue(_currentTopic, out var specialLines))
                {
                    Console.WriteLine("🤖 " + specialLines[_rng.Next(specialLines.Length)]);
                }

                string followUpResponse = responses[_rng.Next(responses.Length)];
                PrintBotResponse(followUpResponse);
                return true;
            }
            return false;
        }


        private static string? DetectSentiment(string input)
        {
            foreach (var kvp in ChatBotData.SentimentKeywords)
            {
                if (Regex.IsMatch(input, $@"\b{Regex.Escape(kvp.Key)}\b"))
                    return kvp.Value;
            }
            return null;
        }



        private static string? DetectTopic(string input)
        {
            foreach (var kvp in ChatBotData.RegexResponses)
            {
                if (Regex.IsMatch(input, kvp.Key))
                {
                    _currentTopic = kvp.Key;

                    // Increment mention count
                    if (!_topicMentionCount.ContainsKey(kvp.Key))
                        _topicMentionCount[kvp.Key] = 1;
                    else
                        _topicMentionCount[kvp.Key]++;

                    if (_userInterest == null && input.Contains("interested") || input.Contains("favorite"))
                    {
                        _userInterest = kvp.Key;
                        string friendly = GetFriendlyTopicName(kvp.Key);
                        PrintBotResponse($"Great! I'll remember that you're interested in {friendly}. It's an important area of cybersecurity.");
                    }

                    return kvp.Value[_rng.Next(kvp.Value.Length)];
                }
            }
            return null;
        }


        private static void RespondToInput(string? sentimentMessage, string? topicResponse)
        {
            Console.WriteLine();

            if (sentimentMessage != null)
                TextEffects.TypeEffect($"I understand that{_userNameMemory}, {sentimentMessage}", 15);

            if (topicResponse != null)
            {
                if (_userInterest != null && _currentTopic == _userInterest && _topicMentionCount.TryGetValue(_userInterest, out int count) && count == 3)
                {
                    if (ChatBotData.PersistentInterestResponses.TryGetValue(_userInterest, out var specialLines))
                    {
                        Console.WriteLine("🤖 " + specialLines[_rng.Next(specialLines.Length)]);
                    }
                }
                else if (_userInterest != null && _currentTopic == _userInterest)
                {
                    string topicLabel = GetFriendlyTopicName(_userInterest);
                    string leadIn = InterestLeadIns[_rng.Next(InterestLeadIns.Length)];
                    Console.WriteLine("🤖 " + string.Format(leadIn, topicLabel));
                }

                PrintBotResponse(topicResponse);
            }
        }




        private static void HandleFallback()
        {
            if (_userInterest != null)
            {
                Console.WriteLine();
                string cleanInterest = GetFriendlyTopicName(_userInterest);
                Console.WriteLine($"🤔 My apoligies, I don't seem to understand though You’ve previously shown interest in {cleanInterest}. Would you like to revisit that?");
                Console.WriteLine();
            }
            else
            {
                PrintBotResponse("🤖 I'm not quite sure how to respond to that. Try asking about passwords, phishing, VPNs, or privacy.");
            }
        }

        private static string GetFriendlyTopicName(string topicRegex)
        {
            return ChatBotData.TopicLabels.TryGetValue(topicRegex, out var label)
                ? label
                : CleanTopicName(topicRegex);
        }

        private static string CleanTopicName(string topicRegex)
        {
            return topicRegex
                .Replace(@"\b", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("|", ", ")
                .Replace("[", "")
                .Replace("]", "")
                .Trim();
        }

        private static void PrintBotResponse(string message)
        {
            Console.WriteLine();

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
