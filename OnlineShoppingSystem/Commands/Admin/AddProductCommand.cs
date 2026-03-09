using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Commands.Admin;

/// <summary>
/// Command for adding a new product to the catalog
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
            var name = InputHelper.ReadNonEmptyString("Enter product name: ");
            if (name.Length > ValidationConstants.MaxProductNameLength || ValidationHelper.ContainsDangerousContent(name))
            {
                ConsoleHelper.DisplayError("Invalid product name.");
                ConsoleHelper.PauseForUser();
                return;
            }

            var description = InputHelper.ReadNonEmptyString("Enter product description: ");
            if (description.Length > ValidationConstants.MaxDescriptionLength || ValidationHelper.ContainsDangerousContent(description))
            {
                ConsoleHelper.DisplayError("Invalid description.");
                ConsoleHelper.PauseForUser();
                return;
            }

            var price = InputHelper.ReadPositiveDecimal("Enter product price: R");
            if (price > ValidationConstants.MaxPrice)
            {
                ConsoleHelper.DisplayError(ValidationConstants.PriceTooHigh);
                ConsoleHelper.PauseForUser();
                return;
            }

            var stockQuantity = InputHelper.ReadPositiveInt("Enter initial stock quantity: ");
            if (stockQuantity > ValidationConstants.MaxStockQuantity)
            {
                ConsoleHelper.DisplayError(ValidationConstants.StockTooHigh);
                ConsoleHelper.PauseForUser();
                return;
            }

            var category = InputHelper.ReadNonEmptyString("Enter product category: ");
            if (category.Length > ValidationConstants.MaxCategoryLength || ValidationHelper.ContainsDangerousContent(category))
            {
                ConsoleHelper.DisplayError("Invalid category.");
                ConsoleHelper.PauseForUser();
                return;
            }

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
