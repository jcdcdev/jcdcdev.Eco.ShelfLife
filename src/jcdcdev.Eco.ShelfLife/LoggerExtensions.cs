using jcdcdev.Eco.Core.Logging;

namespace jcdcdev.Eco.ShelfLife;

public static class LoggerExtensions
{
    public static void LogShelfLife(string objectName, float shelfLifeValue)
    {
        var message = new List<KeyValuePair<string, ConsoleColor>>()
        {
            new($"{objectName}", ConsoleColor.Cyan),
            new(" => ", ConsoleColor.DarkGray),
            new($"{shelfLifeValue:F}", ConsoleColor.DarkGreen)
        };

        Logger.WriteLine(message);
    }
}