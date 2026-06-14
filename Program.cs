using SpaceMission.Interfaces;
using SpaceMission.Services;

ISymbolEncoder encoder = new SymbolEncoder();
IPathFinder pathFinder = new BFSPathfinder(encoder);

MissionControlUI();

try
{
while (true)
{
    string command;
    try {
        command = ConsoleInput.ReadCommand(
            "\nCommands:\n- manual\n- random\n- exit\nEnter a command: ", "manual", "random", "exit");
    }
    catch (EndOfStreamException) {
        break;
    }

    if (command == "exit")
        break;

    try {
        IMapGenerator generator = command == "random" ? BuildRandomGenerator(encoder) : new ConsoleMapReader(encoder);

        MissionControl mission = new MissionControl(encoder, generator, pathFinder);
        mission.completeMission();

        string emailCommand = ConsoleInput.ReadCommand("Email report? (send / skip): ", "send", "skip");
        if (emailCommand == "send")
        {
            IReportSender sender = BuildEmailSender();
            try {
                mission.sendReport(sender);
                Console.WriteLine("Report emailed successfully.");
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to send the email: {ex.Message}");
            }
        }
    }
    catch (Exception ex) {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

Console.WriteLine("Mission control shutting down. Safe travels!");
}
catch (Exception ex)
{
    Console.WriteLine($"Fatal error: {ex.Message}");
}

IMapGenerator BuildRandomGenerator(ISymbolEncoder encoder)
{
    int rows = ConsoleInput.ReadInt("Rows: ", 2, 100);
    int cols = ConsoleInput.ReadInt("Columns: ", 2, 100);
    int astronauts = ConsoleInput.ReadInt("Astronauts [1-3]: ", 1, 3);

    int maxAsteroids = rows * cols - astronauts - 1;
    int asteroids = ConsoleInput.ReadInt($"Asteroids [0-{maxAsteroids}]: ", 0, maxAsteroids);

    return new RandomMapGenerator(encoder, rows, cols, asteroids, astronauts);
}

IReportSender BuildEmailSender()
{
    string from = ConsoleInput.ReadNonEmpty("Sender email: ");
    string password = ConsoleInput.ReadNonEmpty("Sender password: ");
    string to = ConsoleInput.ReadNonEmpty("Receiver email: ");
    return new SmtpEmailSender(from, password, to);
}


void MissionControlUI()
{
    Console.WriteLine("=================== SPACE Mission Control ===================\n");
    Console.WriteLine("Hello, this is our space mission control program.");
    Console.WriteLine("With this program you can find the shortest path");
    Console.WriteLine("from our astronauts to our Space station\n");
    Console.WriteLine("=============================================================");
    Console.WriteLine("                    --- Program usage ---");
    Console.WriteLine("=============================================================");

    Console.WriteLine("- First you need to enter a map. You can write the map");
    Console.WriteLine("  yourself or we can generate a random map for you.\n");

     Console.WriteLine("======================== Commands ==========================");
    Console.WriteLine("  -- manual - type your own cosmic map");
    Console.WriteLine("  -- random - we build a random map from a size you choose");
    Console.WriteLine("  -- exit - shut down mission control\n");
    Console.WriteLine("=============================================================");
    Console.WriteLine("- A cosmic map is built from these symbols:");
    Console.WriteLine("  -- S1, S2, S3  - astronauts (1 to 3 of them)");
    Console.WriteLine("  -- F - the Space Station (exactly one)");
    Console.WriteLine("  -- 0 - open space (safe, costs 1 step)");
    Console.WriteLine("  -- X - asteroid (blocked, cannot enter)");
    Console.WriteLine("  -- D - space debris (passable, costs 2 steps)\n");

    Console.WriteLine("- For a manual map you enter:");
    Console.WriteLine("  -- rows [2-100]\n  -- columns [2-100]\n  -- each row of");
    Console.WriteLine("     space-separated symbols.\n");

    Console.WriteLine("- For a random map you enter:");
    Console.WriteLine("  -- rows\n  -- columns\n  -- number of astronauts [1-3]");
    Console.WriteLine("  -- number of asteroids.\n");

    Console.WriteLine("- We then show, for every astronaut, the shortest path");
    Console.WriteLine("  to the Space Station, marked with '*' (shortest first).");
    Console.WriteLine("  Astronauts with no valid route are reported as lost in");
    Console.WriteLine("  space, at the top of the output.\n");

    Console.WriteLine("- After each mission you can email the report:");
    Console.WriteLine("  -- send - enter sender email, password and receiver email");
    Console.WriteLine("  -- skip - continue without sending\n");

    Console.WriteLine("  * Note: for Gmail, use an App Password (not your normal");
    Console.WriteLine("    account password).");
    Console.WriteLine("=============================================================");
}
