namespace OnlineShoppingSystem.Helpers;

/// <summary>
/// Helper class for reading and validating user input
/// </summary>
public static class InputHelper
{
    /// <summary>
    /// Read an integer from the console with validation
    /// </summary>
    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int result))
            {
                return result;
            }
            ConsoleHelper.DisplayError("Invalid input. Please enter a valid number.");
        }
    }

    /// <summary>
    /// Read a decimal from the console with validation
    /// </summary>
    public static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (decimal.TryParse(Console.ReadLine(), out decimal result))
            {
                return result;
            }
            ConsoleHelper.DisplayError("Invalid input. Please enter a valid decimal number.");
        }
    }

    /// <summary>
    /// Read a string from the console
    /// </summary>
    public static string ReadString(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Read a non-empty string from the console
    /// </summary>
    public static string ReadNonEmptyString(string prompt)
    {
        while (true)
        {
            var input = ReadString(prompt);
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            ConsoleHelper.DisplayError("Input cannot be empty. Please try again.");
        }
    }

    /// <summary>
    /// Read a menu choice within a specific range
    /// </summary>
    public static int ReadMenuChoice(int minOption, int maxOption)
    {
        while (true)
        {
            var choice = ReadInt("\nEnter your choice: ");
            if (choice >= minOption && choice <= maxOption)
            {
                return choice;
            }
            ConsoleHelper.DisplayError($"Invalid choice. Please enter a number between {minOption} and {maxOption}.");
        }
    }

    /// <summary>
    /// Read a positive integer from the console
    /// </summary>
    public static int ReadPositiveInt(string prompt)
    {
        while (true)
        {
            var value = ReadInt(prompt);
            if (value > 0)
            {
                return value;
            }
            ConsoleHelper.DisplayError("Value must be greater than zero.");
        }
    }

    /// <summary>
    /// Read a positive decimal from the console
    /// </summary>
    public static decimal ReadPositiveDecimal(string prompt)
    {
        while (true)
        {
            var value = ReadDecimal(prompt);
            if (value > 0)
            {
                return value;
            }
            ConsoleHelper.DisplayError("Value must be greater than zero.");
        }
    }
}
