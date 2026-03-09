using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;

namespace OnlineShoppingSystem.Commands.Customer;

/// <summary>
/// Command for browsing all available products
/// </summary>
public class BrowseProductsCommand : ICommand
{
    private readonly IProductService _productService;

    public BrowseProductsCommand(IProductService productService)
    {
        _productService = productService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("BROWSE PRODUCTS");

        var products = _productService.GetAllProducts();

        if (!products.Any())
        {
            ConsoleHelper.DisplayWarning("No products available.");
            ConsoleHelper.PauseForUser();
            return;
        }

        ProductDisplayHelper.DisplayProductTable(products);
        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "Browse Products";
}
