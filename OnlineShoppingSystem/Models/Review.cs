namespace OnlineShoppingSystem.Models;

/// <summary>
/// Represents a customer review for a product
/// </summary>
public class Review
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime ReviewDate { get; set; }

    public Review()
    {
        CustomerName = string.Empty;
        Comment = string.Empty;
        ReviewDate = DateTime.Now;
    }
}
