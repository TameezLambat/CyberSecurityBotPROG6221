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
        private static readonly Dictionary<string, int> _topicMentionCount = new();        // Tracks how many times each topic has been mentioned in the conversation

        private static readonly Random _rng = new();// Random number generator for selecting random responses

        // Phrases that indicate the user is asking for more details/follow-up
        private static readonly string[] FollowUps =
        {
            "tell me more", "elaborate", "what else", "why", "how", "expand", "can you explain", "more", "continue"
        };

        // Templates for lead-in phrases
        private static readonly string[] InterestLeadIns =
        {
            "Since you're interested in {0}, here’s something useful:",
            "Here’s something helpful related to {0}:",
            "Let’s explore more about {0}:",
            "This might interest you as someone who cares about {0}:"
        };
        // Memory of user name, current topic in conversation, and user's favorite topic of interest
        private static string? _userNameMemory = null;
        private static string? _currentTopic = null;
        private static string? _userInterest = null;

        //main loops to start chatbot with provided name
        public static void Start(string userName)
        {
            _userNameMemory = userName;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(ChatBotData.MenuText);
            Console.ResetColor();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{userName} › "); //prompt for user name
                Console.ResetColor();

                string input = Console.ReadLine()?.ToLower().Trim() ?? "";
                //blank input
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

                if (InputValidator.IsHelpCommand(input))//return menu
                {
                    ShowMenu();
                    continue;
                }
                // If input is a menu choice number, map to topic key string
                if (InputValidator.TryGetMenuChoice(input, out int n))
                {
                    // Map choice to topic key (like "password", "phishing")
                    input = MapMenuChoiceToTopic(n);
                }
                else if (int.TryParse(input, out _))
                {
                    Console.WriteLine("❌ That number doesn’t match any topic. Please choose 1–5.");
                    continue;
                }

                if (InputValidator.IsGibberish(input))
                {
                    //unclear input
                    HandleGibberish();
                    continue;
                }

                if (HandleFollowUp(input))
                    continue;

                // Try to detect sentiment and topic from user input
                string? sentimentMessage = DetectSentiment(input);
                string? botStatusMessage = DetectBotStatus(input); // bot status
                string? topicResponse = DetectTopic(input);

                if (sentimentMessage != null || botStatusMessage !=null || topicResponse != null)
                {
                    if (botStatusMessage != null)
                    {
                        PrintBotResponse(botStatusMessage); // respond immediately
                        continue;
                    }
                    // attempt to pull from data and respond to sentiment
                    RespondToInput(sentimentMessage, topicResponse);
                    continue;
                }
                //if non found
                HandleFallback();
            }
        }

        private static void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(ChatBotData.MenuText);
            Console.ResetColor();
        }
        //matches input to topic data
        private static string MapMenuChoiceToTopic(int choice) => choice switch
        {
            1 => "password",
            2 => "phishing",
            3 => "safeBrowsing",
            4 => "vpn",
            5 => "privacy",
            _ => ""
        };

        // Handles gibberish input by prompting the user to ask relevant questions
        private static void HandleGibberish()
        {
            Console.WriteLine();
            Console.WriteLine("🤖 That doesn’t look like a question, try asking about passwords, phishing, safe browsing, VPNs, or privacy.");
            Console.WriteLine();
        }
        //methoed to attempt follow up handling
        private static bool HandleFollowUp(string input)
        {
            bool isFollowUp = FollowUps.Any(phrase => input.Contains(phrase)) && _currentTopic != null;
            if (!isFollowUp) return false;
            //increment mention count
            if (_currentTopic != null)
            {
                if (!_topicMentionCount.ContainsKey(_currentTopic))
                    _topicMentionCount[_currentTopic] = 1;
                else
                    _topicMentionCount[_currentTopic]++;
            }

            if (_currentTopic != null &&
                ChatBotData.RegexResponses.TryGetValue(_currentTopic, out var tuple))
            {
                //if topic is mentioned 4 times, print interest observation statement
                if (_userInterest == _currentTopic &&
                    _topicMentionCount.TryGetValue(_currentTopic, out int count) &&
                    count == 4 &&
                    ChatBotData.PersistentInterestResponses.TryGetValue(_currentTopic, out var specialLines))
                {
                    Console.WriteLine("🤖 " + specialLines[_rng.Next(specialLines.Length)]);
                }

                // Otherwise give a random response from the topic advice array
                string followUpResponse = tuple.Responses[_rng.Next(tuple.Responses.Length)];
                PrintBotResponse(followUpResponse);
                return true;
            }
            return false;
        }
        // Detects sentiment keywords in input and returns corresponding response message, if any
        private static string? DetectSentiment(string input)
        {
            foreach (var kvp in ChatBotData.SentimentKeywords)
            {
                if (Regex.IsMatch(input, $@"\b{Regex.Escape(kvp.Key)}\b"))
                    return kvp.Value;
            }
            return null;
        }
        // Detects topic based on regex patterns and returns a topic-specific response
        private static string? DetectTopic(string input)
        {
            foreach (var kvp in ChatBotData.RegexResponses)
            {
                var (pattern, responses) = kvp.Value;
                if (Regex.IsMatch(input, pattern))
                {
                    _currentTopic = kvp.Key;
                    // Track how many times this topic has been mentioned
                    if (!_topicMentionCount.ContainsKey(_currentTopic))
                        _topicMentionCount[_currentTopic] = 1;
                    else
                        _topicMentionCount[_currentTopic]++;
                    // If user indicates interest or favorite, remember their interest
                    if (_userInterest == null && (input.Contains("interested") || input.Contains("favorite")))
                    {
                        _userInterest = _currentTopic;
                        string friendly = GetFriendlyTopicName(_currentTopic);
                        PrintBotResponse($"Great! I'll remember that you're interested in {friendly}. It's an important area of cybersecurity.");
                    }

                    return responses[_rng.Next(responses.Length)];
                }
            }
            return null;
        }
        // Prints sentiment and/or topic advice responses to user with formatting
        private static void RespondToInput(string? sentimentMessage, string? topicResponse)
        {
            Console.WriteLine();

            if (sentimentMessage != null)
                TextEffects.TypeEffect($"I understand that {_userNameMemory}, {sentimentMessage}", 15);

            if (topicResponse != null)
            {
                if (_userInterest != null && _currentTopic == _userInterest &&
                    _topicMentionCount.TryGetValue(_userInterest, out int count) && count == 3)
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
        // Default fallback response when input isn't understood
        private static void HandleFallback()
        {
            if (_userInterest != null)
            {
                Console.WriteLine();
                string cleanInterest = GetFriendlyTopicName(_userInterest);
                Console.WriteLine($"🤔 My apologies, I don't seem to understand though. You’ve previously shown interest in {cleanInterest}. Would you like to revisit that?");
                Console.WriteLine();
            }
            else
            {
                PrintBotResponse("🤖 I'm not quite sure how to respond to that. Try asking about passwords, phishing, VPNs, or privacy.");
            }
        }

        private static string GetFriendlyTopicName(string topicKey)
        {
            return ChatBotData.TopicLabels.TryGetValue(topicKey, out var label)
                ? label
                : CleanTopicName(topicKey);
        }

        // Helper to remove regex meta-characters from topic keys for display
        private static string CleanTopicName(string topicKey)
        {
            return topicKey
                .Replace(@"\b", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("|", ", ")
                .Replace("[", "")
                .Replace("]", "")
                .Trim();
        }
        private static string? DetectBotStatus(string input)
        {
            foreach (var kvp in ChatBotData.BotStatusResponses)
            {
                if (input.Contains(kvp.Key))
                {
                    return kvp.Value[_rng.Next(kvp.Value.Length)];
                }
            }
            return null;
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
