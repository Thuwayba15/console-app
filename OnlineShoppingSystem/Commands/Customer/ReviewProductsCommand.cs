using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Commands.Customer;

/// <summary>
/// Command for leaving product reviews
/// </summary>
public class ReviewProductsCommand : ICommand
{
    private readonly Models.Customer _customer;
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly IReviewService _reviewService;

    public ReviewProductsCommand(
        Models.Customer customer,
        IProductService productService,
        IOrderService orderService,
        IReviewService reviewService)
    {
        _customer = customer;
        _productService = productService;
        _orderService = orderService;
        _reviewService = reviewService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("REVIEW PRODUCTS");

        try
        {
            var orders = _orderService.GetCustomerOrders(_customer.Id);
            if (!orders.Any())
            {
                ConsoleHelper.DisplayWarning("You need to purchase products before leaving reviews.");
                ConsoleHelper.PauseForUser();
                return;
            }

            var purchasedProductIds = orders
                .SelectMany(o => o.Items)
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            if (!purchasedProductIds.Any())
            {
                ConsoleHelper.DisplayWarning("You haven't purchased any products yet.");
                ConsoleHelper.PauseForUser();
                return;
            }

            var reviewableProducts = purchasedProductIds
                .Select(id => _productService.GetProductById(id))
                .Where(p => p != null)
                .ToList();

            ProductDisplayHelper.DisplayReviewableProducts(reviewableProducts!);

            var productIdToReview = InputHelper.ReadInt("Enter Product ID to review (0 to cancel): ");

            if (productIdToReview == 0) return;

            if (!purchasedProductIds.Contains(productIdToReview))
            {
                ConsoleHelper.DisplayError("You can only review products you have purchased.");
                ConsoleHelper.PauseForUser();
                return;
            }

            var productToReview = _productService.GetProductById(productIdToReview);
            if (productToReview == null)
            {
                ConsoleHelper.DisplayError("Product not found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            Console.WriteLine($"\nReviewing: {productToReview.Name}");
            var rating = InputHelper.ReadInt("Enter rating (1-5 stars): ");

            if (!ValidationHelper.IsValidRating(rating))
            {
                ConsoleHelper.DisplayError("Rating must be between 1 and 5.");
                ConsoleHelper.PauseForUser();
                return;
            }

            var comment = InputHelper.ReadString("Enter your review comment: ");

            if (string.IsNullOrWhiteSpace(comment))
            {
                ConsoleHelper.DisplayWarning("Review comment cannot be empty.");
                ConsoleHelper.PauseForUser();
                return;
            }

            _reviewService.AddReview(productIdToReview, _customer.Id, _customer.Username, rating, comment);

            ConsoleHelper.DisplaySuccess("Review added successfully!");
            ConsoleHelper.DisplayInfo($"Product: {productToReview.Name}");
            ConsoleHelper.DisplayInfo($"Rating: {rating}/5 stars");
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error adding review: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "Review Products";
}
