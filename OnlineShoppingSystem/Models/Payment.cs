namespace OnlineShoppingSystem.Models;

/// <summary>
/// Represents a payment transaction
/// </summary>
public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public bool IsSuccessful { get; set; }

    public Payment()
    {
        PaymentDate = DateTime.Now;
    }
}
