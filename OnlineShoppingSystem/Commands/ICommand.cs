namespace OnlineShoppingSystem.Commands;

/// <summary>
/// Command interface for menu actions
/// Implements the Command Pattern to encapsulate menu operations
/// Each command represents a single action (e.g., checkout, view cart, add product)
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Execute the command
    /// </summary>
    void Execute();

    /// <summary>
    /// Get the name/description of this command (useful for logging)
    /// </summary>
    string GetName();
}
