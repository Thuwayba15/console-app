using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Services;

/// <summary>
/// Service for managing product reviews
/// </summary>
public class ReviewService : IReviewService
{
    private readonly AppDataStore _dataStore;

    public ReviewService()
    {
        _dataStore = AppDataStore.Instance;
    }

    public Review AddReview(int productId, int customerId, string customerName, int rating, string comment)
    {
        if (rating < 1 || rating > 5)
        {
            throw new ArgumentException("Rating must be between 1 and 5.");
        }

        var product = _dataStore.Products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        var review = new Review
        {
            Id = _dataStore.GetNextReviewId(),
            ProductId = productId,
            CustomerId = customerId,
            CustomerName = customerName,
            Rating = rating,
            Comment = comment
        };

        _dataStore.Reviews.Add(review);
        return review;
    }

    public List<Review> GetProductReviews(int productId)
    {
        return _dataStore.Reviews
            .Where(r => r.ProductId == productId)
            .OrderByDescending(r => r.ReviewDate)
            .ToList();
    }

    public List<Review> GetAllReviews()
    {
        return _dataStore.Reviews
            .OrderByDescending(r => r.ReviewDate)
            .ToList();
    }
}
