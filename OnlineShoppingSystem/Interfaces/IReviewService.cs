using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Interfaces;

/// <summary>
/// Interface for product review operations
/// </summary>
public interface IReviewService
{
    /// <summary>
    /// Add a review for a product
    /// </summary>
    Review AddReview(int productId, int customerId, string customerName, int rating, string comment);

    /// <summary>
    /// Get all reviews for a product
    /// </summary>
    List<Review> GetProductReviews(int productId);

    /// <summary>
    /// Get all reviews
    /// </summary>
    List<Review> GetAllReviews();
}
