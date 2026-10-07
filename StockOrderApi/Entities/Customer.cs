namespace StockOrderApi.Entities;

public class Customer
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}