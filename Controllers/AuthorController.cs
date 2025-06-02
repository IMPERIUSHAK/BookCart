using BookCart;
using BookCart.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<AuthorsController> _logger;

    public AuthorsController(AppDbContext context, ILogger<AuthorsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAutors()
    {
        var authors = await _context.Authors.ToListAsync();
        return Ok(authors);
    }
    [HttpGet("getbyid/{AuthorId}")]
    public async Task<IActionResult> GetAuthorById(int AuthorId)
    {
        var author = await _context.Authors.FindAsync(AuthorId);
        if (author == null)
        {
            _logger.LogWarning($"Author with ID {AuthorId} not found");
            return NotFound();
        }
        return Ok(author);
    }
    [HttpPost("add")]
    public async Task<IActionResult> AddAuthor([FromBody] Author author)
    {
        if (author == null)
        {
            _logger.LogWarning("Attempted to add null author");
            return BadRequest("Author data is required");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid author data received");
            return BadRequest(ModelState);
        }

        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAuthorById), new { id = author.AuthorId }, author);
    }
    [HttpDelete("del/{AuthorId}")]
    public async Task<IActionResult> DeleteAuthor(int AuthorId)
    {
        var author = await _context.Authors.FindAsync(AuthorId);
        if (author == null)
        {
            _logger.LogWarning($"Author with ID {AuthorId} no found");
            return NotFound();
        }
        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
        return Ok();
    }
    [HttpPut("edit/{AuthorId}")]
    public async Task<IActionResult> UpdateAuthor(int AuthorId, [FromBody] Author author)
    {
        var exAuthor = await _context.Authors.FindAsync(AuthorId);
        if (exAuthor == null)
        {
            _logger.LogWarning($"Author with ID {AuthorId} not found");
            return NotFound();
        }

        exAuthor.Name = author.Name;
        exAuthor.Bio = author.Bio;
        if (author.Books != null)
        {
            exAuthor.Books = author.Books;
        }
        await _context.SaveChangesAsync();
        _logger.LogWarning($"author with ID{AuthorId} changed");
        return Ok(author);
    }
}
