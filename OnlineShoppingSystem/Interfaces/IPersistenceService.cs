namespace OnlineShoppingSystem.Interfaces;

/// <summary>
/// Interface for persisting data to JSON files
/// </summary>
public interface IPersistenceService
{
    /// <summary>
    /// Save all application data to JSON files
    /// </summary>
    void SaveData();

    /// <summary>
    /// Load all application data from JSON files
    /// </summary>
    void LoadData();
}
