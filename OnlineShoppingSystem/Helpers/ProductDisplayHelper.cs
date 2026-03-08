using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Helpers;

/// <summary>
/// Helper class for displaying product information
/// </summary>
public static class ProductDisplayHelper
{
    /// <summary>
    /// Display a list of products in a formatted table
    /// </summary>
    public static void DisplayProductTable(List<Product> products)
    {
        Console.WriteLine("\n{0,-5} {1,-30} {2,-12} {3,-10} {4,-20}", "ID", "Name", "Price", "Stock", "Category");
        Console.WriteLine(new string('-', 80));

        foreach (var product in products.OrderBy(p => p.Category).ThenBy(p => p.Name))
        {
            var stockDisplay = product.StockQuantity <= 10 
                ? $"{product.StockQuantity} (LOW)" 
                : product.StockQuantity.ToString();
            
            var nameDisplay = product.Name.Length > 28 
                ? product.Name.Substring(0, 28) + ".." 
                : product.Name;

            Console.WriteLine("{0,-5} {1,-30} R{2,-11:F2} {3,-10} {4,-20}",
                product.Id,
                nameDisplay,
                product.Price,
                stockDisplay,
                product.Category);
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Display a simple list of products for review selection
    /// </summary>
    public static void DisplayReviewableProducts(List<Product> products)
    {
        Console.WriteLine("\nProducts you can review:");
        Console.WriteLine("{0,-5} {1,-30} {2,-12}", "ID", "Product Name", "Price");
        Console.WriteLine(new string('-', 50));

        foreach (var product in products)
        {
            var nameDisplay = product.Name.Length > 28 
                ? product.Name.Substring(0, 28) + ".." 
                : product.Name;

            Console.WriteLine("{0,-5} {1,-30} R{2,-11:F2}",
                product.Id,
                nameDisplay,
                product.Price);
        }

        Console.WriteLine();
    }
}
