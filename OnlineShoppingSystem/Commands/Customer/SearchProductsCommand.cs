using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;

namespace OnlineShoppingSystem.Commands.Customer;

/// <summary>
/// Command for searching products by name, description, or category
/// </summary>
public class SearchProductsCommand : ICommand
{
    private readonly IProductService _productService;

    public SearchProductsCommand(IProductService productService)
    {
        _productService = productService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("SEARCH PRODUCTS");

        var searchTerm = InputHelper.ReadString("Enter search term (name, description, or category): ");

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            ConsoleHelper.DisplayWarning("Search term cannot be empty.");
            ConsoleHelper.PauseForUser();
            return;
        }

        var products = _productService.SearchProducts(searchTerm);

        if (!products.Any())
        {
            ConsoleHelper.DisplayWarning($"No products found matching '{searchTerm}'.");
            ConsoleHelper.PauseForUser();
            return;
        }

        ConsoleHelper.DisplaySuccess($"Found {products.Count} product(s) matching '{searchTerm}':");
        ProductDisplayHelper.DisplayProductTable(products);
        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "Search Products";
}
