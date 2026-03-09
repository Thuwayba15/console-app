using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Commands.Customer;

/// <summary>
/// Command for adding a product to the shopping cart
/// </summary>
public class AddToCartCommand : ICommand
{
    private readonly Models.Customer _customer;
    private readonly ICartService _cartService;
    private readonly IProductService _productService;

    public AddToCartCommand(Models.Customer customer, ICartService cartService, IProductService productService)
    {
        _customer = customer;
        _cartService = cartService;
        _productService = productService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("ADD PRODUCT TO CART");

        try
        {
            var products = _productService.GetAllProducts();
            if (!products.Any())
            {
                ConsoleHelper.DisplayWarning("No products available.");
                ConsoleHelper.PauseForUser();
                return;
            }

            ProductDisplayHelper.DisplayProductTable(products);

            var productId = InputHelper.ReadInt("Enter Product ID: ");
            var product = _productService.GetProductById(productId);

            if (product == null)
            {
                ConsoleHelper.DisplayError("Product not found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            if (product.StockQuantity == 0)
            {
                ConsoleHelper.DisplayError($"{product.Name} is out of stock.");
                ConsoleHelper.PauseForUser();
                return;
            }

            Console.WriteLine($"\n{product.Name} - R{product.Price:F2} (Available: {product.StockQuantity})");
            var quantity = InputHelper.ReadPositiveInt("Enter quantity: ");

            var success = _cartService.AddToCart(_customer.Id, productId, quantity);

            if (success)
            {
                ConsoleHelper.DisplaySuccess($"Added {quantity} x {product.Name} to cart.");
            }
            else
            {
                ConsoleHelper.DisplayError("Failed to add product to cart.");
            }
        }
        catch (InvalidOperationException ex)
        {
            ConsoleHelper.DisplayError(ex.Message);
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error adding to cart: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "Add to Cart";
}
