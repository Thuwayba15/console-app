using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;

namespace OnlineShoppingSystem.Commands.Admin;

public class UpdateProductCommand : ICommand
{
    private readonly IProductService _productService;
    private readonly IPersistenceService _persistenceService;

    public UpdateProductCommand(IProductService productService, IPersistenceService persistenceService)
    {
        _productService = productService;
        _persistenceService = persistenceService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("UPDATE PRODUCT");
        
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
            var productId = InputHelper.ReadInt("Enter Product ID to update (0 to cancel): ");
            
            if (productId == 0) return;

            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                ConsoleHelper.DisplayError("Product not found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            Console.WriteLine($"\nCurrent: {product.Name} - R{product.Price:F2}");
            Console.WriteLine("Enter new values (press Enter to keep current):");
            
            var name = InputHelper.ReadString($"Name [{product.Name}]: ");
            if (string.IsNullOrWhiteSpace(name)) name = product.Name;
            
            var desc = InputHelper.ReadString($"Description [{product.Description}]: ");
            if (string.IsNullOrWhiteSpace(desc)) desc = product.Description;
            
            var priceStr = InputHelper.ReadString($"Price [R{product.Price:F2}]: ");
            var price = string.IsNullOrWhiteSpace(priceStr) ? product.Price : decimal.Parse(priceStr);
            
            var category = InputHelper.ReadString($"Category [{product.Category}]: ");
            if (string.IsNullOrWhiteSpace(category)) category = product.Category;

            var success = _productService.UpdateProduct(productId, name, desc, price, product.StockQuantity, category);

            if (success)
            {
                ConsoleHelper.DisplaySuccess("Product updated successfully!");
                _persistenceService.SaveData();
            }
            else
            {
                ConsoleHelper.DisplayError("Failed to update product.");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "Update Product";
}

public class DeleteProductCommand : ICommand
{
    private readonly IProductService _productService;
    private readonly IPersistenceService _persistenceService;

    public DeleteProductCommand(IProductService productService, IPersistenceService persistenceService)
    {
        _productService = productService;
        _persistenceService = persistenceService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("DELETE PRODUCT");

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
            var productId = InputHelper.ReadInt("Enter Product ID to delete (0 to cancel): ");
            
            if (productId == 0) return;

            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                ConsoleHelper.DisplayError("Product not found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            Console.Write($"\nDelete '{product.Name}'? (yes/no): ");
            if (Console.ReadLine()?.Trim().ToLower() != "yes")
            {
                ConsoleHelper.DisplayInfo("Cancelled.");
                ConsoleHelper.PauseForUser();
                return;
            }

            if (_productService.DeleteProduct(productId))
            {
                ConsoleHelper.DisplaySuccess($"Product '{product.Name}' deleted!");
                _persistenceService.SaveData();
            }
            else
            {
                ConsoleHelper.DisplayError("Failed to delete product.");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "Delete Product";
}

public class RestockProductCommand : ICommand
{
    private readonly IProductService _productService;
    private readonly IPersistenceService _persistenceService;

    public RestockProductCommand(IProductService productService, IPersistenceService persistenceService)
    {
        _productService = productService;
        _persistenceService = persistenceService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("RESTOCK PRODUCT");

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
            var productId = InputHelper.ReadInt("Enter Product ID to restock (0 to cancel): ");
            
            if (productId == 0) return;

            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                ConsoleHelper.DisplayError("Product not found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            Console.WriteLine($"\nProduct: {product.Name}");
            Console.WriteLine($"Current Stock: {product.StockQuantity}");

            var quantity = InputHelper.ReadPositiveInt("Enter quantity to add: ");

            if (_productService.RestockProduct(productId, quantity))
            {
                var updated = _productService.GetProductById(productId);
                ConsoleHelper.DisplaySuccess("Product restocked!");
                ConsoleHelper.DisplayInfo($"New stock: {updated?.StockQuantity}");
                _persistenceService.SaveData();
            }
            else
            {
                ConsoleHelper.DisplayError("Failed to restock.");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "Restock Product";
}

public class ViewProductsCommand : ICommand
{
    private readonly IProductService _productService;

    public ViewProductsCommand(IProductService productService)
    {
        _productService = productService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("ALL PRODUCTS");

        var products = _productService.GetAllProducts();

        if (!products.Any())
        {
            ConsoleHelper.DisplayWarning("No products available.");
        }
        else
        {
            ProductDisplayHelper.DisplayProductTable(products);
            ConsoleHelper.DisplayInfo($"Total products: {products.Count}");
        }

        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "View Products";
}

public class ViewLowStockCommand : ICommand
{
    private readonly IProductService _productService;

    public ViewLowStockCommand(IProductService productService)
    {
        _productService = productService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("LOW STOCK PRODUCTS");

        var lowStockProducts = _productService.GetLowStockProducts(10);

        if (!lowStockProducts.Any())
        {
            ConsoleHelper.DisplaySuccess("All products are adequately stocked!");
        }
        else
        {
            ConsoleHelper.DisplayWarning($"Found {lowStockProducts.Count} product(s) with low stock:");
            ProductDisplayHelper.DisplayProductTable(lowStockProducts);
        }

        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "View Low Stock";
}
