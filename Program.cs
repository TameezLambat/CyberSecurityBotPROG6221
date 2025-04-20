using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Media;
using System.Text.RegularExpressions;

namespace CyberSecurityChatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Bot";
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            PlayVoiceGreeting(); // Play welcome audio (from resources folder)
            DisplayAsciiArt(); // Show startup banner

            Console.Write("Please enter your name: ");
            string userName = Console.ReadLine();
            Console.WriteLine();
            DisplayDivider();
            TypeEffect($"Hello, {userName}! I'm your Cybersecurity Awareness Bot.", 35); // Personalized welcome
            DisplayDivider();

            ChatLoop(userName); // Start chat loop
        }

        // Play a pre-recorded welcome message from 
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

        // Display ASCII art banner
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

        // Print a visual divider (easy on the eyes)
        static void DisplayDivider()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('-', 55));
            Console.ResetColor();
        }

        // Simulates typing effect
        static void TypeEffect(string message, int delay = 20)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        // Main chatbot interaction loop (accepts username parameter)
        static void ChatLoop(string userName)
        {
            // Advice content strings 
            var passwordAdvice = string.Join("\n", new[]
            {
                "1.) Use long, unique passwords for each site.",
                "   - A strong password should be at least 12 characters long and contain a mix of upper/lowercase letters, numbers, and symbols, or is simply something that's very hard to guess.",
                "   - Example: 'A3#r9lX7mQ' is far stronger than 'password123'.",
                "2.) Avoid using the same password for multiple sites.",
                "   - If one account gets breached, others won't be compromised.",
                "3.) Use a password manager.",
                "   - It helps generate and store complex passwords securely."
            });

            var phishingAdvice = string.Join("\n", new[]
            {
                "1.) Be cautious with urgent or unexpected emails.",
                "   - Phishing emails often try to create panic to trick you into clicking links or giving away info.",
                "2.) Always verify the sender's email address.",
                "   - Look out for strange characters or unfamiliar domains.",
                "3.) Never download attachments from unknown sources.",
                "   - These could contain malware or viruses."
            });

            var browsingAdvice = string.Join("\n", new[]
            {
                "1.) Always use HTTPS websites.",
                "   - HTTPS encrypts your connection, protecting your data from interception.",
                "2.) Avoid clicking pop-ups and suspicious ads.",
                "   - These can redirect to malicious websites or files.",
                "3.) Use a VPN on public Wi-Fi.",
                "   - It adds a layer of encryption to keep your browsing private."
            });

            // Keywords mapped to advice responses using regex patterns
            var regexResponses = new Dictionary<string, string>
            {
                { @"\b(password|credentials|login)\b", passwordAdvice },
                { @"\b(phishing|scam|fake email|fraud)\b", phishingAdvice },
                { @"\b(https|vpn|wifi|browsing|internet)\b", browsingAdvice },
                { @"\b(hello|hi|hey|yo)\b", "👋 Hello there! I'm here to help you stay safe online. Ask me about passwords, phishing, or safe browsing!" },
                { @"\b(how are you|how's it going|how do you do)\b", "😊 I'm just a bunch of code, but I'm running smoothly! Let's keep your digital life secure." },
                { @"\b(what are you|what is your purpose|what do you do|who are you)\b", "🤖 I'm your Cybersecurity Awareness Bot, here to teach you the best practices to stay safe online!" }
            };

            // Display options for predefined topics
            var topicOptions = new Dictionary<int, string>
            {
                { 1, "Password Safety" },
                { 2, "Phishing & Scams" },
                { 3, "Safe Browsing" },
                { 4, "Help (Show available prompts)" },
                { 5, "Exit" }
            };

            while (true)
            {
                // Display menu options
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("What would you like to learn about?");
                foreach (var option in topicOptions)
                {
                    Console.WriteLine($"{option.Key}. {option.Value}");
                }
                Console.ResetColor();

                // Prompt for input
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

                if (int.TryParse(input, out int choice) && topicOptions.ContainsKey(choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            TypeEffect("🔐 Password Safety Tips:\n", 25);
                            Console.ResetColor();
                            TypeEffect(passwordAdvice + "\n", 10);
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            TypeEffect("🎣 Phishing Alert:\n", 25);
                            Console.ResetColor();
                            TypeEffect(phishingAdvice + "\n", 10);
                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            TypeEffect("🌐 Safe Browsing 101:\n", 25);
                            Console.ResetColor();
                            TypeEffect(browsingAdvice + "\n", 10);
                            break;
                        case 4:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            TypeEffect("📚 Help Menu — Try Asking Me About:\n", 25);
                            Console.ResetColor();
                            TypeEffect("- Passwords (e.g., 'How do I create a strong password?')\n" +
                                       "- Phishing (e.g., 'How can I spot a scam email?')\n" +
                                       "- Safe Browsing (e.g., 'Is public Wi-Fi safe to use?')\n" +
                                       "- Say 'hello', 'how are you', or ask my purpose\n" +
                                       "- Type 'exit' or choose option 5 to leave\n", 10);
                            break;
                        case 5:
                            Console.WriteLine("👋 Stay safe out there!");
                            return;
                        default:
                            break;
                    }
                }
                else
                {
                    bool matched = false;
                    foreach (var pattern in regexResponses)
                    {
                        if (Regex.IsMatch(input, pattern.Key))
                        {
                            TypeEffect(pattern.Value);
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
