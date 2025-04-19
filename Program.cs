using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Media;

namespace CyberSecurityChatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Bot";
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            PlayVoiceGreeting();
            DisplayAsciiArt();

            Console.Write("Please enter your name: ");
            string userName = Console.ReadLine();
            Console.WriteLine();
            DisplayDivider();
            TypeEffect($"Hello, {userName}! I'm your Cybersecurity Awareness Bot.", 35);
            DisplayDivider();

            ChatLoop(userName);
        }

        static void PlayVoiceGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer(@"Resources\\ttsmaker-file-2025-4-19-1-24-16.wav");
                player.PlaySync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ Could not play audio: " + ex.Message);
            }
        }

        static void DisplayAsciiArt()
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

        static void DisplayDivider()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('-', 55));
            Console.ResetColor();
        }

        static void TypeEffect(string message, int delay = 20)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        static void ChatLoop(string userName)
        {
            var topicTips = new Dictionary<int, (string Title, string[] Tips)>
            {
                { 1, ("🔐 Password Safety Tips:", new[]
                    {
                        "1.) Use long, unique passwords for each site.",
                        "   - A strong password should be at least 12 characters long and contain a mix of upper/lowercase letters, numbers, and symbols.",
                        "   - Example: 'A3#r9lX7mQ' is stronger than 'password123'.",
                        "2.) Avoid using the same password for multiple sites.",
                        "   - If one account gets breached, others won't be compromised.",
                        "3.) Use a password manager.",
                        "   - It helps generate and store complex passwords securely."
                    })
                },
                { 2, ("🎣 Phishing Alert:", new[]
                    {
                        "1.) Be cautious with urgent or unexpected emails.",
                        "   - Phishing emails often try to create panic to trick you into clicking links or giving away info.",
                        "2.) Always verify the sender's email address.",
                        "   - Look out for strange characters or unfamiliar domains.",
                        "3.) Never download attachments from unknown sources.",
                        "   - These could contain malware or viruses."
                    })
                },
                { 3, ("🌐 Safe Browsing 101:", new[]
                    {
                        "1.) Always use HTTPS websites.",
                        "   - HTTPS encrypts your connection, protecting your data from interception.",
                        "2.) Avoid clicking pop-ups and suspicious ads.",
                        "   - These can redirect to malicious websites.",
                        "3.) Use a VPN on public Wi-Fi.",
                        "   - It adds a layer of encryption to keep your browsing private."
                    })
                }
            };

            var keywordResponses = new Dictionary<string[], string>
            {
                {
                    new[] { "password", "strong password", "login", "credentials" },
                    string.Join("\n", topicTips[1].Tips)
                },
                {
                    new[] { "phishing", "scam", "fake email", "link" },
                    string.Join("\n", topicTips[2].Tips)
                },
                {
                    new[] { "safe browsing", "internet", "web", "wifi", "surfing" },
                    string.Join("\n", topicTips[3].Tips)
                },
                {
                    new[] { "hello", "hi", "hey", "yo" },
                    "👋 Hello there! I'm here to help you learn how to stay safe online. Ask me about passwords, phishing, or safe browsing!"
                },
                {
                    new[] { "how are you", "how's it going", "how do you do" },
                    "😊 I'm just a bunch of code, but I'm running smoothly! Let's keep your digital life secure."
                },
                {
                    new[] { "what are you", "what is your purpose", "what do you do", "who are you" },
                    "🤖 I'm your Cybersecurity Awareness Bot, designed to teach and guide you on best practices for staying safe online!"
                }
            };

            var topicOptions = new Dictionary<int, string>
            {
                { 1, "Password Safety" },
                { 2, "Phishing & Scams" },
                { 3, "Safe Browsing" },
                { 4, "Have a custom question? Ask away!" }
            };

            while (true)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("What would you like to learn about?");
                foreach (var option in topicOptions)
                {
                    Console.WriteLine($"{option.Key}. {option.Value}");
                }
                Console.ResetColor();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{userName}, type a number or ask a question: ");
                Console.ResetColor();

                string input = Console.ReadLine()?.ToLower().Trim();
                Console.WriteLine();
                DisplayDivider();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("⚠️ Please enter something.");
                    continue;
                }

                if (input == "exit" || input == "quit")
                {
                    Console.WriteLine("👋 Stay safe out there!");
                    break;
                }

                if (int.TryParse(input, out int choice) && topicTips.ContainsKey(choice))
                {
                    var (title, tips) = topicTips[choice];
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(title);
                    foreach (var tip in tips)
                    {
                        Console.WriteLine(tip);
                    }
                    Console.ResetColor();
                }
                else
                {
                    bool matched = false;
                    foreach (var entry in keywordResponses)
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
                        TypeEffect("🤔 I didn’t quite understand that. Try asking about passwords, phishing, or safe browsing.");
                    }
                }

                DisplayDivider();
            }
        }
    }
}
