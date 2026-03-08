using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;

namespace OnlineShoppingSystem.Menus;

/// <summary>
/// Main menu for user registration, login, and exit
/// </summary>
public class MainMenu
{
    private readonly IAuthService _authService;

    public MainMenu(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Display and handle the main menu
    /// </summary>
    public void Show()
    {
        while (true)
        {
            ConsoleHelper.DisplayHeader("ONLINE SHOPPING SYSTEM");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");

            var choice = InputHelper.ReadMenuChoice(1, 3);

            switch (choice)
            {
                case 1:
                    // Registration will be implemented in next milestone
                    ConsoleHelper.DisplayInfo("Registration feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 2:
                    // Login will be implemented in next milestone
                    ConsoleHelper.DisplayInfo("Login feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 3:
                    ConsoleHelper.DisplayInfo("Thank you for using Online Shopping System!");
                    return;
            }
        }
    }
}
