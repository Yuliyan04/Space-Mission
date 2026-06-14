namespace SpaceMission.Services;

public static class ConsoleInput
{
    public static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string? line = Console.ReadLine();
            if (line == null)
                throw new EndOfStreamException("Unexpected end of input.");

            if (int.TryParse(line.Trim(), out int value) && value >= min && value <= max)
                return value;

            Console.WriteLine($"Invalid input. Enter a whole number between {min} and {max}.");
        }
    }

    public static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? line = Console.ReadLine();
            if (line == null)
                throw new EndOfStreamException("Unexpected end of input.");

            if (!string.IsNullOrWhiteSpace(line))
                return line.Trim();

            Console.WriteLine("Value cannot be empty.");
        }
    }

    public static string ReadCommand(string prompt, params string[] allowed)
    {
        while (true)
        {
            Console.Write(prompt);
            string? line = Console.ReadLine();
            if (line == null)
                throw new EndOfStreamException("Unexpected end of input.");

            string command = line.Trim().ToLower();
            foreach (string option in allowed)
                if (command == option)
                    return command;

            Console.WriteLine($"Unknown command. Options: {string.Join(", ", allowed)}.");
        }
    }
}
