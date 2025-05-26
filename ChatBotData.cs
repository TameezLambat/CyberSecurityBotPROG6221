namespace CyberSecurityChatBot.Data
{
    public static class ChatBotData
    {
        public static readonly Dictionary<string, string> SentimentKeywords = new()
        {
           // Worried / Anxious
        { "worried", "😟 It's okay to feel that way. Cybersecurity can be scary, but I'm here to help you through it." },
        { "anxious", "😟 You’re not alone—security concerns can be overwhelming. Let's take it one step at a time." },
        { "nervous", "😟 Feeling nervous is normal. I’ve got your back—ask anything, anytime." },
        { "scared", "😟 Cyber threats can sound intense, but don’t worry. You're in a safe place to learn and protect yourself Online." },

        // Confused / Lost
        { "confused", "🤔 I can help clarify! Ask me anything about passwords, phishing, or safe browsing." },
        { "unclear", "🤔 Let’s walk through it together—what part can I help explain better?" },
        { "lost", "🤔 That’s totally fine. Let me know where you're stuck and I’ll help guide you." },
        { "unsure", "🤔 Uncertainty is part of learning. I can simplify things—just ask!" },

        // Frustrated / Overwhelmed
        { "frustrated", "😣 It’s frustrating when things don’t make sense. Let's break it down together." },
        { "annoyed", "😣 I hear you. Let’s sort it out together and turn this into progress." },
        { "overwhelmed", "😣 Cybersecurity can feel like a lot, but we’ll take it one step at a time." },
        { "fed up", "😣 Don’t give up—sometimes all it takes is a fresh perspective. Let’s tackle it." },

        // Curious / Interested
        { "curious", "🧐 I love curiosity! Let’s explore cybersecurity tips you might find interesting." },
        { "wondering", "🧐 Great question! Ask me anything you’ve been wondering about." },
        { "thinking", "🧐 Thinking ahead is smart. What would you like to know more about?" }

        };

        public static readonly string[] PasswordAdvice =
        {
            string.Join("\n", new[]
            {
                "🌟 Use long, unique and strong passwords for each site.",
                "📛 Avoid using the same password for multiple sites.",
                "🔐 Use a reputable password manager to store and generate strong passwords."
            }),
            string.Join("\n", new[]
            {
                "🔑  Keep every password unique and 12+ chars.",
                "💡  Mix upper/lowercase letters, numbers, and symbols.",
                "🔒  One account hacked ≠ all accounts hacked.",
                "✅  A password manager remembers the complexity for you."
            }),
            string.Join("\n", new[]
            {
                "🔏 Think of your password like a toothbrush — don’t share it and change it regularly!",
                "🧩 Use passphrases made of unrelated words for easier memorization and strong security.",
                "🎯 Avoid common words and predictable substitutions like 'P@ssw0rd'.",
                "🔑 Consider enabling two-factor authentication wherever possible.",
                "💾 Back up your passwords securely in case you forget them."
            })
        };

        public static readonly string[] PhishingAdvice =
        {
            string.Join("\n", new[]
            {
                "⚠️ Be cautious with urgent or unexpected emails.",
                "✅ Verify the sender’s email address carefully.",
                "📎 Never download attachments from unknown sources."
            }),
            string.Join("\n", new[]
            {
                "⚠️ Always check the sender’s email carefully before clicking.",
                "🚫 Don’t trust links asking for personal info unexpectedly.",
                "🔍 Hover over links to see their true destination.",
                "📞 When in doubt, call the company directly using a known number."
            }),
            string.Join("\n", new[]
            {
                "🚨 Phishing emails often create a sense of urgency — pause and think before acting.",
                "🔬 Look out for spelling mistakes or odd phrasing; scammers often slip up.",
                "📧 Legit companies usually don’t ask for sensitive info via email.",
                "💡 Use browser tools or email filters to help detect phishing attempts.",
                "⚔️ Report suspicious emails to your IT or security team to protect others."
            })
        };

        public static readonly string[] BrowsingAdvice =
        {
            string.Join("\n", new[]
            {
                "🚀 Always use HTTPS websites.",
                "💬 Avoid clicking pop‑ups and suspicious ads.",
                "🧑‍💻 Use a VPN when browsing on public Wi‑Fi."
            }),
            string.Join("\n", new[]
            {
                "🌐 Prefer websites that start with HTTPS for secure browsing.",
                "❌ Avoid clicking on suspicious pop-ups or ads.",
                "🔐 Use a VPN on public Wi-Fi to encrypt your traffic.",
                "🛡️ Keep your browser updated to protect against vulnerabilities."
            }),
            string.Join("\n", new[]
            {
                "🛑 Don’t ignore browser warnings about unsafe websites.",
                "👓 Review website URLs carefully—watch for slight misspellings or odd domains.",
                "💼 Use separate browsers or profiles for personal and sensitive browsing.",
                "🔄 Regularly update your privacy settings and clear your browsing history.",
                "🌟 Bookmark frequently used sites to avoid mistyping URLs."
            })
        };

        public static readonly string[] VPNAdvice =
        {
            string.Join("\n", new[]
            {
                "🔒 Use a reliable VPN to encrypt your internet traffic.",
                "🌍 VPNs help mask your IP address and protect your location.",
                "⚠️ Avoid free VPNs as they may log your data or inject ads.",
                "✅ Choose a VPN with a strict no-logs policy.",
                "🔄 Use VPNs especially on public Wi-Fi networks to stay safe."
            }),
            string.Join("\n", new[]
            {
                "🛡️ VPNs protect your privacy and help bypass censorship.",
                "📶 VPNs can slightly slow your internet connection due to encryption.",
                "⚙️ Configure your VPN to start automatically for continuous protection.",
                "🔒 Combine VPN use with secure browsers and HTTPS sites for best security."
            })
        };

        public static readonly string[] PrivacyAdvice =
        {
            string.Join("\n", new[]
            {
                "🔐 Review and adjust your privacy settings on social media.",
                "👀 Be cautious about what personal information you share online.",
                "📱 Limit app permissions to only what’s necessary.",
                "🕵️‍♂️ Use privacy-focused browsers and search engines.",
                "🚫 Avoid oversharing to reduce risks of identity theft."
            }),
            string.Join("\n", new[]
            {
                "🔎 Regularly check and delete old accounts you no longer use.",
                "🔒 Use strong passwords and 2FA to protect your accounts.",
                "📢 Be aware of data collection policies before installing apps.",
                "🧹 Clear cookies and cache frequently to protect your browsing privacy."
            })
        };
        public static readonly Dictionary<string, (string Pattern, string[] Responses)> RegexResponses = new()
{
    { "password", (@"\b(password|credentials|login|pass key)\b", PasswordAdvice) },
    { "phishing", (@"\b(phishing|scam|fake email|fraud)\b", PhishingAdvice) },
    { "safeBrowsing", (@"\b(safeBrowsing|https|browsing|internet|safe browsing|secure connection)\b", BrowsingAdvice) },
    { "vpn", (@"\b(vpn|virtual private network)\b", VPNAdvice) },
    { "privacy", (@"\b(privacy|personal info|data protection|private)\b", PrivacyAdvice) }
};

        public static readonly Dictionary<string, string> TopicLabels = new()
{
    { "password", "passwords" },
    { "phishing", "phishing and scams" },
    { "safeBrowsing", "safe browsing" },
    { "vpn", "VPNs" },
    { "privacy", "privacy" }
};

        public static readonly Dictionary<string, string[]> BotStatusResponses = new()
{
    { "how are you", new[] {
        "I'm just a bunch of code, but I'm functioning well! 😄",
        "All systems operational! How can I assist you today?",
        "Feeling cyber-secure as always. Thanks for asking!",
        "I'm doing great, thanks! Ready to help you stay safe online." }
    },
    { "what's up", new[] {
        "Not much, just analyzing threats and giving advice. How about you?",
        "Same old, same old — protecting data and giving tips!",
        "All good here! Just waiting to chat about cybersecurity. 🔐" }
    },
    { "how's it going", new[] {
        "Going well on the digital front! How can I help?",
        "No malware in sight — it's a good day!",
        "All smooth sailing in cyberspace. What brings you here today?" }
    }
};

        public static readonly Dictionary<string, string[]> PersistentInterestResponses = new()
{
    { "password", new[] { "🔁 You’ve brought up passwords a few times — it’s great that you're focused on this. Here’s another tip:" } },
    { "phishing", new[] { "🔁 It’s clear phishing and scams matter to you. Staying vigilant is smart — here's more advice:" } },
    { "safeBrowsing", new[] { "🔁 You’ve revisited browsing safety a lot — it’s worth mastering. Check this out:" } },
    { "vpn", new[] { "🔁 You’re really digging into VPNs — that’s awesome. One more set of tips for the road:" } },
    { "privacy", new[] { "🔁 You’ve asked about privacy multiple times — here’s another set of advice:" } }
};


        public const string MenuText =
            "\n1. Password Safety\n" +
            "2. Phishing & Scams\n" +
            "3. Safe Browsing\n" +
            "4. VPNs\n" +
            "5. Privacy\n" +
            "Type 1-5, or just ask naturally. Type 'help' to see this menu again.\n";
    }
}

