using BookCart;
using BookCart.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[Controller]")]

public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<AuthorsController> _logger;

    public UserController(AppDbContext context, ILogger<AuthorsController> logger)
    {
        _context = context;
        _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

}