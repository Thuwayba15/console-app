using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Validators;

namespace OnlineShoppingSystem.Commands.Admin;

/// <summary>
/// Command for adding a new product to the catalog
/// Uses ProductValidator for consistent validation
/// </summary>
public class AddProductCommand : ICommand
{
    private readonly IProductService _productService;
    private readonly IPersistenceService _persistenceService;

    public AddProductCommand(IProductService productService, IPersistenceService persistenceService)
    {
        _productService = productService;
        _persistenceService = persistenceService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("ADD NEW PRODUCT");

        try
        {
            // Get and validate product name
            var name = InputHelper.ReadNonEmptyString("Enter product name: ");
            var nameValidation = ProductValidator.ValidateName(name);
            if (!nameValidation.IsValid)
            {
                ConsoleHelper.DisplayError(nameValidation.ErrorMessage);
                ConsoleHelper.PauseForUser();
                return;
            }

            // Get and validate product description
            var description = InputHelper.ReadNonEmptyString("Enter product description: ");
            var descValidation = ProductValidator.ValidateDescription(description);
            if (!descValidation.IsValid)
            {
                ConsoleHelper.DisplayError(descValidation.ErrorMessage);
                ConsoleHelper.PauseForUser();
                return;
            }

            // Get and validate price
            var price = InputHelper.ReadPositiveDecimal("Enter product price: R");
            var priceValidation = ProductValidator.ValidatePrice(price);
            if (!priceValidation.IsValid)
            {
                ConsoleHelper.DisplayError(priceValidation.ErrorMessage);
                ConsoleHelper.PauseForUser();
                return;
            }

            // Get and validate stock quantity
            var stockQuantity = InputHelper.ReadPositiveInt("Enter initial stock quantity: ");
            var stockValidation = ProductValidator.ValidateStockQuantity(stockQuantity);
            if (!stockValidation.IsValid)
            {
                ConsoleHelper.DisplayError(stockValidation.ErrorMessage);
                ConsoleHelper.PauseForUser();
                return;
            }

            // Get and validate category
            var category = InputHelper.ReadNonEmptyString("Enter product category: ");
            var categoryValidation = ProductValidator.ValidateCategory(category);
            if (!categoryValidation.IsValid)
            {
                ConsoleHelper.DisplayError(categoryValidation.ErrorMessage);
                ConsoleHelper.PauseForUser();
                return;
            }

            // Add product
            var product = _productService.AddProduct(name, description, price, stockQuantity, category);

            if (product != null)
            {
                ConsoleHelper.DisplaySuccess($"Product '{product.Name}' added successfully!");
                ConsoleHelper.DisplayInfo($"Product ID: {product.Id}");
                _persistenceService.SaveData();
            }
            else
            {
                ConsoleHelper.DisplayError("Failed to add product.");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error adding product: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "Add Product";
}
