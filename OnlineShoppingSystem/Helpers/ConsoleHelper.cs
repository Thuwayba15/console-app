namespace OnlineShoppingSystem.Helpers;

/// <summary>
/// Helper class for console display utilities
/// </summary>
public static class ConsoleHelper
{
    /// <summary>
    /// Display a header with formatting
    /// </summary>
    public static void DisplayHeader(string title)
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine($"  {title}");
        Console.WriteLine("========================================\n");
    }

    /// <summary>
    /// Display a success message
    /// </summary>
    public static void DisplaySuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n? {message}");
        Console.ResetColor();
    }

    /// <summary>
    /// Display an error message
    /// </summary>
    public static void DisplayError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n? {message}");
        Console.ResetColor();
    }

    /// <summary>
    /// Display a warning message
    /// </summary>
    public static void DisplayWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n? {message}");
        Console.ResetColor();
    }

    /// <summary>
    /// Display an info message
    /// </summary>
    public static void DisplayInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n? {message}");
        Console.ResetColor();
    }

    /// <summary>
    /// Pause and wait for user to press any key
    /// </summary>
    public static void PauseForUser()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}
