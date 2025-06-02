using BookCart;
using BookCart.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[Controller]")]

public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<AuthorsController> _logger;

    public UsersController(AppDbContext context, ILogger<AuthorsController> logger)
    {
        _context = context;
        _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var authors = await _context.Users.ToListAsync();
        return Ok(authors);
    }
    
}