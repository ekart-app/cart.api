namespace cart.api.Entities;

public class BasketItem
{
    public string ProductId { get; set; }

    public string ProductName { get; set; }

    public int Quantity { get; set; }

    public double Amount { get; set; }
}