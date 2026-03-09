using OnlineShoppingSystem.Factories;
using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Enums;

namespace OnlineShoppingSystem.Menus;

/// <summary>
/// Main menu for user registration, login, and exit
/// </summary>
public class MainMenu
{
    private readonly IAuthService _authService;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    private readonly IPaymentService _paymentService;
    private readonly IReviewService _reviewService;
    private readonly IReportService _reportService;
    private readonly IPersistenceService _persistenceService;

    public MainMenu(
        IAuthService authService,
        IProductService productService,
        ICartService cartService,
        IOrderService orderService,
        IPaymentService paymentService,
        IReviewService reviewService,
        IReportService reportService,
        IPersistenceService persistenceService)
    {
        _authService = authService;
        _productService = productService;
        _cartService = cartService;
        _orderService = orderService;
        _paymentService = paymentService;
        _reviewService = reviewService;
        _reportService = reportService;
        _persistenceService = persistenceService;
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
                    HandleRegistration();
                    break;
                case 2:
                    HandleLogin();
                    break;
                case 3:
                    ConsoleHelper.DisplayInfo("Thank you for using Online Shopping System!");
                    return;
            }
        }
    }

    /// <summary>
    /// Handle user registration
    /// </summary>
    private void HandleRegistration()
    {
        ConsoleHelper.DisplayHeader("USER REGISTRATION");

        try
        {
            // Get user type
            Console.WriteLine("Select account type:");
            Console.WriteLine("1. Customer");
            Console.WriteLine("2. Administrator");
            var accountTypeChoice = InputHelper.ReadMenuChoice(1, 2);
            bool isAdmin = accountTypeChoice == 2;

            // Get username
            string username = InputHelper.ReadNonEmptyString("Enter username: ");
            
            if (username.Length > ValidationConstants.MaxUsernameLength)
            {
                ConsoleHelper.DisplayError($"Username cannot exceed {ValidationConstants.MaxUsernameLength} characters.");
                ConsoleHelper.PauseForUser();
                return;
            }

            if (ValidationHelper.ContainsDangerousContent(username))
            {
                ConsoleHelper.DisplayError("Username contains invalid characters or patterns.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Check if username already exists
            if (_authService.UsernameExists(username))
            {
                ConsoleHelper.DisplayError("Username already exists. Please try a different username.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Get email
            string email = InputHelper.ReadNonEmptyString("Enter email: ");
            
            if (email.Length > ValidationConstants.MaxEmailLength)
            {
                ConsoleHelper.DisplayError($"Email cannot exceed {ValidationConstants.MaxEmailLength} characters.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Validate email format
            if (!ValidationHelper.IsValidEmail(email))
            {
                ConsoleHelper.DisplayError("Invalid email format. Email must contain @ and a domain.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Check if email already exists
            if (_authService.EmailExists(email))
            {
                ConsoleHelper.DisplayError("Email already exists. Please try a different email.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Get password
            string password = InputHelper.ReadNonEmptyString("Enter password: ");
            
            if (password.Length < ValidationConstants.MinPasswordLength)
            {
                ConsoleHelper.DisplayError(ValidationConstants.PasswordTooShort);
                ConsoleHelper.PauseForUser();
                return;
            }
            
            if (password.Length > ValidationConstants.MaxPasswordLength)
            {
                ConsoleHelper.DisplayError(ValidationConstants.PasswordTooLong);
                ConsoleHelper.PauseForUser();
                return;
            }

            // Confirm password
            string confirmPassword = InputHelper.ReadNonEmptyString("Confirm password: ");

            if (password != confirmPassword)
            {
                ConsoleHelper.DisplayError("Passwords do not match. Please try again.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Register the user
            var newUser = _authService.Register(username, email, password, isAdmin);

            if (newUser != null)
            {
                string accountType = isAdmin ? "Administrator" : "Customer";
                ConsoleHelper.DisplaySuccess($"{accountType} account created successfully!");
                ConsoleHelper.DisplayInfo($"Username: {username}");
                ConsoleHelper.DisplayInfo("You can now login with your credentials.");
            }
            else
            {
                ConsoleHelper.DisplayError("Registration failed. Please try again.");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Registration error: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    /// <summary>
    /// Handle user login
    /// </summary>
    private void HandleLogin()
    {
        ConsoleHelper.DisplayHeader("USER LOGIN");

        try
        {
            // Get username
            string username = InputHelper.ReadNonEmptyString("Enter username: ");
            
            if (username.Length > ValidationConstants.MaxUsernameLength)
            {
                ConsoleHelper.DisplayError($"Username cannot exceed {ValidationConstants.MaxUsernameLength} characters.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Get password
            string password = InputHelper.ReadNonEmptyString("Enter password: ");
            
            if (password.Length > ValidationConstants.MaxPasswordLength)
            {
                ConsoleHelper.DisplayError(ValidationConstants.PasswordTooLong);
                ConsoleHelper.PauseForUser();
                return;
            }

            // Attempt login
            var user = _authService.Login(username, password);

            if (user == null)
            {
                ConsoleHelper.DisplayError("Invalid username or password.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Login successful
            ConsoleHelper.DisplaySuccess($"Welcome, {user.Username}!");

            // Use Factory Pattern to create the appropriate menu based on user role
            var menu = MenuFactory.CreateMenu(
                user,
                _productService,
                _cartService,
                _orderService,
                _paymentService,
                _reviewService,
                _reportService,
                _persistenceService);

            // Show the menu
            if (menu is CustomerMenu customerMenu)
            {
                customerMenu.Show();
            }
            else if (menu is AdministratorMenu adminMenu)
            {
                adminMenu.Show();
            }

            // Save data after logout
            _persistenceService.SaveData();
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Login error: {ex.Message}");
            ConsoleHelper.PauseForUser();
        }
    }
}
