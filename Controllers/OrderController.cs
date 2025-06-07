using System.Xml;
using BookCart;
using BookCart.data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<OrderController> _logger;

    public OrderController(AppDbContext context, ILogger<OrderController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var getOrders = await _context.Orders
            .Include(c => c.OrderItems)
        .ToListAsync();
        if (getOrders == null)
        {
            return NotFound();
        }

        return Ok(getOrders);
    }
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(int orderId)
    {
        var getOrder = await _context.Carts
            .Include(c => c.CartItems)
        .FirstOrDefaultAsync();

        if (getOrder == null)
        {
            return NotFound();
        }

        return Ok(getOrder);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto dto)
    {
        var order = new Order
        {
            UserId = dto.UserId,
            OrderDate = dto.OrderDate,
            Status = dto.Status,
            ShippingAddress = dto.ShippingAddress
        };
        if (dto.Items?.Count > 0)
        {
            order.OrderItems = dto.Items.Select(item => new OrderItem
            {
                OrderId = item.OrderId,
                BookId = item.BookId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList();
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return Ok(order);
    }
}

