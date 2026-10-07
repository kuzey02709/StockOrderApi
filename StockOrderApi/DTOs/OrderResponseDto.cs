namespace StockOrderApi.DTOs;

public class OrderResponseDto
{
    public int Id { get; set; }
    public DateTime OrderDate
    {
        get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string CustomerCompanyName { get; set; } = string.Empty;
    public List<OrderItemDto> OrderItems { get; set; } = new ();
}

public class OrderItemDto
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity
    {
        get; set; }
    public decimal UnitPrice
    {
        get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
}