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
    public async Task<IActionResult> GetAllAuthors()
    {
       var authors = await _context.Authors
            .Select(a => new
            {
                a.AuthorId,
                a.Name,
                a.Bio,
                BookIds = a.Books.Select(b => b.BookId),
                BookName = a.Books.Select(b => b.Title)
            })
        .ToListAsync();
        return Ok(authors);
    }

    [HttpGet("getbyid/{AuthorId}")]
    public async Task<IActionResult> GetAuthorById(int AuthorId)
    {
       var author = await _context.Authors
        .Where(a => a.AuthorId == AuthorId)
        .Select(a => new
        {
            a.AuthorId,
            a.Name,
            a.Bio,
            BookId = a.Books.Select(b => b.BookId),
            BookName = a.Books.Select(b => b.Title)
        })
        .FirstOrDefaultAsync();
        if (author == null)
        {
            _logger.LogWarning($"Author with ID {AuthorId} not found");
            return NotFound();
        }
        return Ok(author);
    }
    [HttpPost("add")]
    public async Task<IActionResult> AddAuthor([FromBody] AuthorDto authorDto)
    {
        var author = new Author
        {
            Name = authorDto.Name,
            Bio = authorDto.Bio,
            Books = new List<Book>()
        };
        
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetAuthorById), new { AuthorId = author.AuthorId }, author);
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
    public async Task<IActionResult> UpdateAuthor(int AuthorId, [FromBody] AuthorDto author)
    {
        var exAuthor = await _context.Authors.FindAsync(AuthorId);
        if (exAuthor == null)
        {
            _logger.LogWarning($"Author with ID {AuthorId} not found");
            return NotFound();
        }

        exAuthor.Name = author.Name;
        exAuthor.Bio = author.Bio;
        await _context.SaveChangesAsync();
        _logger.LogWarning($"author with ID{AuthorId} changed");
        return Ok(exAuthor);
    }
}
