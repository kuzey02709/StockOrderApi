using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockOrderApi.Data;
using StockOrderApi.DTOs;
using StockOrderApi.Entities;

namespace StockOrderApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrders()
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
            .Select(o => new OrderResponseDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                CustomerCompanyName = o.Customer != null ? o.Customer.CompanyName : "Müşteri Bilgisi Yok",
                OrderItems = o.OrderItems.Select(i => new OrderItemDto
                {
                    ProductName = i.Product != null ? i.Product.Name : "Ürün Bilgisi Yok",
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            })
            .ToListAsync();

        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponseDto>> GetOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
            .Where(o => o.Id == id)
            .Select(o => new OrderResponseDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                CustomerCompanyName = o.Customer != null ? o.Customer.CompanyName : "Müşteri Bilgisi Yok",
                OrderItems = o.OrderItems.Select(i => new OrderItemDto
                {
                    ProductName = i.Product != null ? i.Product.Name : "Ürün Bilgisi Yok",
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (order == null)
        {
            return NotFound($"ID: {id} olan sipariş bulunamadı.");
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createDto)
    {
        if (createDto.OrderItems == null || !createDto.OrderItems.Any())
        {
            return BadRequest("Siparişte en az bir ürün olmalıdır.");
        }

        var order = new Order
        {
            CustomerId = createDto.CustomerId,
            OrderDate = DateTime.UtcNow,
            Status = "Completed",
            OrderItems = new List<OrderItem>()
        };

        decimal totalAmount = 0;

        foreach (var itemDto in createDto.OrderItems)
        {
            var product = await _context.Products.FindAsync(itemDto.ProductId);

            if (product == null)
            {
                return BadRequest($"ID: {itemDto.ProductId} olan ürün bulunamadı.");
            }

            if (product.StockQuantity < itemDto.Quantity)
            {
                return BadRequest($"Yetersiz stok! '{product.Name}' ürününden stokta sadece {product.StockQuantity} adet var.");
            }

            product.StockQuantity -= itemDto.Quantity;

            var orderItem = new OrderItem
            {
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price
            };

            order.OrderItems.Add(orderItem);
            totalAmount += orderItem.UnitPrice * orderItem.Quantity;
        }

        order.TotalAmount = totalAmount;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, new { message = $"Sipariş #{order.Id} oluşturuldu.", orderId = order.Id });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound($"ID: {id} olan sipariş bulunamadı.");
        }

        foreach (var item in order.OrderItems)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product != null)
            {
                product.StockQuantity += item.Quantity;
            }
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Sipariş #{id} başarıyla iptal edildi ve ürün stokları iade edildi." });
    }
}