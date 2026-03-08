using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Menus;

/// <summary>
/// Customer menu for browsing, shopping, and managing orders
/// </summary>
public class CustomerMenu
{
    private readonly Customer _customer;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    private readonly IPaymentService _paymentService;
    private readonly IReviewService _reviewService;

    public CustomerMenu(
        Customer customer,
        IProductService productService,
        ICartService cartService,
        IOrderService orderService,
        IPaymentService paymentService,
        IReviewService reviewService)
    {
        _customer = customer;
        _productService = productService;
        _cartService = cartService;
        _orderService = orderService;
        _paymentService = paymentService;
        _reviewService = reviewService;
    }

    /// <summary>
    /// Display and handle the customer menu
    /// </summary>
    public void Show()
    {
        while (true)
        {
            ConsoleHelper.DisplayHeader($"CUSTOMER MENU - Welcome, {_customer.Username}!");
            Console.WriteLine("1. Browse Products");
            Console.WriteLine("2. Search Products");
            Console.WriteLine("3. Add Product to Cart");
            Console.WriteLine("4. View Cart");
            Console.WriteLine("5. Update Cart");
            Console.WriteLine("6. Checkout");
            Console.WriteLine("7. View Wallet Balance");
            Console.WriteLine("8. Add Wallet Funds");
            Console.WriteLine("9. View Order History");
            Console.WriteLine("10. Track Orders");
            Console.WriteLine("11. Review Products");
            Console.WriteLine("12. Logout");

            var choice = InputHelper.ReadMenuChoice(1, 12);

            switch (choice)
            {
                case 1:
                    BrowseProducts();
                    break;
                case 2:
                    SearchProducts();
                    break;
                case 3:
                    AddProductToCart();
                    break;
                case 4:
                    ViewCart();
                    break;
                case 5:
                    UpdateCart();
                    break;
                case 6:
                    Checkout();
                    break;
                case 7:
                    ViewWalletBalance();
                    break;
                case 8:
                    AddWalletFunds();
                    break;
                case 9:
                    ViewOrderHistory();
                    break;
                case 10:
                    TrackOrders();
                    break;
                case 11:
                    ReviewProducts();
                    break;
                case 12:
                    ConsoleHelper.DisplaySuccess("Logged out successfully.");
                    ConsoleHelper.PauseForUser();
                    return;
            }
        }
    }

    #region Product Browsing and Search

    /// <summary>
    /// Browse all available products
    /// </summary>
    private void BrowseProducts()
    {
        ConsoleHelper.DisplayHeader("BROWSE PRODUCTS");

        var products = _productService.GetAllProducts();

        if (!products.Any())
        {
            ConsoleHelper.DisplayWarning("No products available.");
            ConsoleHelper.PauseForUser();
            return;
        }

        DisplayProductList(products);
        ConsoleHelper.PauseForUser();
    }

    /// <summary>
    /// Search products by name, description, or category
    /// </summary>
    private void SearchProducts()
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
        DisplayProductList(products);
        ConsoleHelper.PauseForUser();
    }

    /// <summary>
    /// Display a list of products in a formatted table
    /// </summary>
    private void DisplayProductList(List<Product> products)
    {
        ProductDisplayHelper.DisplayProductTable(products);
    }

    #endregion

    #region Shopping Cart Operations

    /// <summary>
    /// Add a product to the shopping cart
    /// </summary>
    private void AddProductToCart()
    {
        ConsoleHelper.DisplayHeader("ADD PRODUCT TO CART");

        try
        {
            // Show available products
            var products = _productService.GetAllProducts();
            if (!products.Any())
            {
                ConsoleHelper.DisplayWarning("No products available.");
                ConsoleHelper.PauseForUser();
                return;
            }

            DisplayProductList(products);

            // Get product ID
            var productId = InputHelper.ReadInt("Enter Product ID: ");
            var product = _productService.GetProductById(productId);

            if (product == null)
            {
                ConsoleHelper.DisplayError("Product not found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Check if product is in stock
            if (product.StockQuantity == 0)
            {
                ConsoleHelper.DisplayError($"{product.Name} is out of stock.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Get quantity
            Console.WriteLine($"\n{product.Name} - R{product.Price:F2} (Available: {product.StockQuantity})");
            var quantity = InputHelper.ReadPositiveInt("Enter quantity: ");

            // Add to cart
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

    /// <summary>
    /// View the current shopping cart
    /// </summary>
    private void ViewCart()
    {
        ConsoleHelper.DisplayHeader("YOUR SHOPPING CART");

        var cart = _cartService.GetCart(_customer.Id);

        if (cart == null || !cart.Items.Any())
        {
            ConsoleHelper.DisplayWarning("Your cart is empty.");
            ConsoleHelper.PauseForUser();
            return;
        }

        OrderDisplayHelper.DisplayCart(cart);
        ConsoleHelper.PauseForUser();
    }

    /// <summary>
    /// Update cart item quantities or remove items
    /// </summary>
    private void UpdateCart()
    {
        ConsoleHelper.DisplayHeader("UPDATE CART");

        var cart = _cartService.GetCart(_customer.Id);

        if (cart == null || !cart.Items.Any())
        {
            ConsoleHelper.DisplayWarning("Your cart is empty.");
            ConsoleHelper.PauseForUser();
            return;
        }

        // Display current cart
        ViewCart();

        try
        {
            var productId = InputHelper.ReadInt("Enter Product ID to update (0 to cancel): ");

            if (productId == 0)
            {
                return;
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem == null)
            {
                ConsoleHelper.DisplayError("Product not found in cart.");
                ConsoleHelper.PauseForUser();
                return;
            }

            Console.WriteLine($"\nCurrent quantity: {cartItem.Quantity}");
            Console.WriteLine("Enter new quantity (0 to remove item):");
            var newQuantity = InputHelper.ReadInt("New quantity: ");

            if (newQuantity < 0)
            {
                ConsoleHelper.DisplayError("Quantity cannot be negative.");
                ConsoleHelper.PauseForUser();
                return;
            }

            if (newQuantity == 0)
            {
                var removed = _cartService.RemoveFromCart(_customer.Id, productId);
                if (removed)
                {
                    ConsoleHelper.DisplaySuccess($"Removed {cartItem.ProductName} from cart.");
                }
            }
            else
            {
                var updated = _cartService.UpdateCartItem(_customer.Id, productId, newQuantity);
                if (updated)
                {
                    ConsoleHelper.DisplaySuccess($"Updated {cartItem.ProductName} quantity to {newQuantity}.");
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            ConsoleHelper.DisplayError(ex.Message);
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error updating cart: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    #endregion

    #region Checkout and Payment

    /// <summary>
    /// Process checkout and create an order
    /// </summary>
    private void Checkout()
    {
        ConsoleHelper.DisplayHeader("CHECKOUT");

        var cart = _cartService.GetCart(_customer.Id);

        if (cart == null || !cart.Items.Any())
        {
            ConsoleHelper.DisplayError("Your cart is empty. Add products before checking out.");
            ConsoleHelper.PauseForUser();
            return;
        }

        // Display cart summary
        OrderDisplayHelper.DisplayCheckoutSummary(cart);

        // Check wallet balance
        var currentBalance = _paymentService.GetWalletBalance(_customer.Id);
        Console.WriteLine($"\nYour Wallet Balance: R{currentBalance:F2}");

        if (currentBalance < cart.GetTotal())
        {
            ConsoleHelper.DisplayError($"Insufficient funds. You need R{cart.GetTotal() - currentBalance:F2} more.");
            ConsoleHelper.DisplayInfo("Please add funds to your wallet (option 8) and try again.");
            ConsoleHelper.PauseForUser();
            return;
        }

        // Confirm checkout
        Console.Write("\nConfirm checkout? (yes/no): ");
        var confirmation = Console.ReadLine()?.Trim().ToLower();

        if (confirmation != "yes" && confirmation != "y")
        {
            ConsoleHelper.DisplayInfo("Checkout cancelled.");
            ConsoleHelper.PauseForUser();
            return;
        }

        try
        {
            // Create order (this handles payment, stock reduction, and cart clearing)
            var order = _orderService.CreateOrder(_customer.Id);

            if (order != null)
            {
                ConsoleHelper.DisplaySuccess($"Order #{order.Id} created successfully!");
                ConsoleHelper.DisplayInfo($"Total: ${order.TotalAmount:F2}");
                ConsoleHelper.DisplayInfo($"New Wallet Balance: ${_paymentService.GetWalletBalance(_customer.Id):F2}");
                ConsoleHelper.DisplayInfo($"Order Status: {order.Status}");
                
                // Update customer reference
                _customer.WalletBalance = _paymentService.GetWalletBalance(_customer.Id);
            }
            else
            {
                ConsoleHelper.DisplayError("Failed to create order.");
            }
        }
        catch (InvalidOperationException ex)
        {
            ConsoleHelper.DisplayError(ex.Message);
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Checkout error: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    /// <summary>
    /// View current wallet balance
    /// </summary>
    private void ViewWalletBalance()
    {
        ConsoleHelper.DisplayHeader("WALLET BALANCE");

        var balance = _paymentService.GetWalletBalance(_customer.Id);
        _customer.WalletBalance = balance;

        Console.WriteLine($"\nCurrent Balance: ${balance:F2}");
        ConsoleHelper.PauseForUser();
    }

    /// <summary>
    /// Add funds to wallet
    /// </summary>
    private void AddWalletFunds()
    {
        ConsoleHelper.DisplayHeader("ADD WALLET FUNDS");

        var currentBalance = _paymentService.GetWalletBalance(_customer.Id);
        Console.WriteLine($"Current Balance: ${currentBalance:F2}\n");

        try
        {
            var amount = InputHelper.ReadPositiveDecimal("Enter amount to add: $");

            var success = _paymentService.AddFunds(_customer.Id, amount);

            if (success)
            {
                var newBalance = _paymentService.GetWalletBalance(_customer.Id);
                _customer.WalletBalance = newBalance;
                
                ConsoleHelper.DisplaySuccess($"Added ${amount:F2} to your wallet.");
                ConsoleHelper.DisplayInfo($"New Balance: ${newBalance:F2}");
            }
            else
            {
                ConsoleHelper.DisplayError("Failed to add funds.");
            }
        }
        catch (ArgumentException ex)
        {
            ConsoleHelper.DisplayError(ex.Message);
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error adding funds: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    #endregion

    #region Order Management

    /// <summary>
    /// View order history
    /// </summary>
    private void ViewOrderHistory()
    {
        ConsoleHelper.DisplayHeader("ORDER HISTORY");

        var orders = _orderService.GetCustomerOrders(_customer.Id);

        if (!orders.Any())
        {
            ConsoleHelper.DisplayWarning("You have no orders yet.");
            ConsoleHelper.PauseForUser();
            return;
        }

        OrderDisplayHelper.DisplayOrderHistory(orders);
        ConsoleHelper.PauseForUser();
    }

    /// <summary>
    /// Track specific orders
    /// </summary>
    private void TrackOrders()
    {
        ConsoleHelper.DisplayHeader("TRACK ORDERS");

        var orders = _orderService.GetCustomerOrders(_customer.Id);

        if (!orders.Any())
        {
            ConsoleHelper.DisplayWarning("You have no orders to track.");
            ConsoleHelper.PauseForUser();
            return;
        }

        // Display summary
        OrderDisplayHelper.DisplayOrderSummaryTable(orders);

        // Track specific order
        var orderId = InputHelper.ReadInt("Enter Order ID to view details (0 to cancel): ");

        if (orderId == 0)
        {
            return;
        }

        var selectedOrder = orders.FirstOrDefault(o => o.Id == orderId);

        if (selectedOrder == null)
        {
            ConsoleHelper.DisplayError("Order not found.");
            ConsoleHelper.PauseForUser();
            return;
        }

        // Display order details
        OrderDisplayHelper.DisplayOrderDetails(selectedOrder);
        ConsoleHelper.PauseForUser();
    }

    #endregion

    #region Product Reviews

    /// <summary>
    /// Leave a review for a product
    /// </summary>
    private void ReviewProducts()
    {
        ConsoleHelper.DisplayHeader("REVIEW PRODUCTS");

        try
        {
            // Check if customer has any orders
            var orders = _orderService.GetCustomerOrders(_customer.Id);
            if (!orders.Any())
            {
                ConsoleHelper.DisplayWarning("You need to purchase products before leaving reviews.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Get all unique products from customer's orders
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

            // Display products that can be reviewed
            var reviewableProducts = purchasedProductIds
                .Select(id => _productService.GetProductById(id))
                .Where(p => p != null)
                .ToList();

            ProductDisplayHelper.DisplayReviewableProducts(reviewableProducts!);

            // Get product to review
            var productIdToReview = InputHelper.ReadInt("Enter Product ID to review (0 to cancel): ");

            if (productIdToReview == 0)
            {
                return;
            }

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

            // Get rating
            Console.WriteLine($"\nReviewing: {productToReview.Name}");
            var rating = InputHelper.ReadInt("Enter rating (1-5 stars): ");

            if (!ValidationHelper.IsValidRating(rating))
            {
                ConsoleHelper.DisplayError("Rating must be between 1 and 5.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Get comment
            var comment = InputHelper.ReadString("Enter your review comment: ");

            if (string.IsNullOrWhiteSpace(comment))
            {
                ConsoleHelper.DisplayWarning("Review comment cannot be empty.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Add review
            var review = _reviewService.AddReview(
                productIdToReview,
                _customer.Id,
                _customer.Username,
                rating,
                comment);

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

    #endregion
}
