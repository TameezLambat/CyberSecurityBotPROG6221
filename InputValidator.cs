namespace CyberSecurityChatBot.Utils
{
    public static class InputValidator
    {
        // Check for exit command
        public static bool IsQuitCommand(string input) =>
            input is "exit" or "quit" or "bye";

        // Check for menu/help command
        public static bool IsHelpCommand(string input) =>
            input is "help" or "menu" or "?";

        // Check if user typed a valid menu number
        public static bool TryGetMenuChoice(string input, out int choice)
        {
            if (int.TryParse(input, out choice) && choice is >= 1 and <= 5)
                return true;

            choice = -1;
            return false;
        }

        // Basic gibberish or placeholder check
        public static bool IsGibberish(string input)
        {
            return input.Length < 3 || input == "asdf" || input == "123" || input == "..." || input.Contains("???");
        }
    }
}
