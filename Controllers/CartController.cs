using BookCart;
using BookCart.data;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<CartController> _logger;

    public CartController(AppDbContext context, ILogger<CartController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(int userId)
    {
        var getcart = await _context.Carts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Book)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (getcart == null)
        {
            return NotFound();
        }

        return Ok(getcart);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartDto dto)
    {

        var cart = new Cart { UserId = dto.UserId };
        
        if (dto.Items?.Count > 0)
        {
            cart.CartItems = dto.Items.Select(item => new CartItem
            {
                BookId = item.BookId,
                Quantity = item.Quantity
            }).ToList();
        }

        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCart), new { userId = cart.UserId }, cart);
    }
}
