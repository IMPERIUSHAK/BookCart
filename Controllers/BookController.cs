using BookCart;
using BookCart.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<BooksController> _logger;
    public BooksController(AppDbContext context, ILogger<BooksController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _context.Books
        .Select(b => new BookDto { BookId = b.BookId, Title = b.Title })
        .ToListAsync();
        return Ok(books);
    }

    [HttpGet("getbyid/{id}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            _logger.LogWarning($"Book with ID {id} not found");
            return NotFound();
        }

        return Ok(new BookDto { BookId = book.BookId, Title = book.Title });
    }

    [HttpPost ("add/")]
    public async Task<IActionResult> AdBook([FromBody] Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBookById), new { id = book.BookId }, book);
    }

    [HttpDelete("del/{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            _logger.LogWarning($"Book with ID {id} not found");
            return NotFound();
        }
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("edit/{id}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
    {
        var existingBook = await _context.Books.FindAsync(id);
        if (existingBook == null)
        {
            _logger.LogWarning($"Book with ID {id} not found");
            return NotFound();
        }

        _context.Entry(book).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        _logger.LogWarning($"Book with ID {id} changed successful");

        return Ok(new BookDto { BookId = existingBook.BookId, Title = existingBook.Title }); 
    }
}