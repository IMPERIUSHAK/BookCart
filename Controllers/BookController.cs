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
        var books = await _context.Books.ToListAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            _logger.LogWarning($"Book with ID {id} not found");
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> AddBook([FromBody] BookCreateDto bookDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var authorExists = await _context.Authors.AnyAsync(a => a.AuthorId == bookDto.AuthorId);
        var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == bookDto.CategoryId);

        if (!authorExists || !categoryExists)
            return NotFound(authorExists ? "Category not found" : "Author not found");

        var newBook = new Book
        {
            Title = bookDto.Title,
            Price = bookDto.Price,
            Description = bookDto.Description,
            AuthorId = bookDto.AuthorId,
            CategoryId = bookDto.CategoryId
        };

        _context.Books.Add(newBook);

       
        var author = await _context.Authors.FindAsync(bookDto.AuthorId);
        if (author != null)
        {
            author.Books.Add(newBook);
        }

        await _context.SaveChangesAsync();
        return Ok(newBook);
    }


    [HttpDelete("{id}")]
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] BookCreateDto bookDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingBook = await _context.Books.FindAsync(id);
        if (existingBook == null)
        {
            _logger.LogWarning($"Book with ID {id} not found");
            return NotFound();
        }

        existingBook.Title = bookDto.Title;
        existingBook.Price = bookDto.Price;
        existingBook.Description = bookDto.Description;
        existingBook.AuthorId = bookDto.AuthorId;
        existingBook.CategoryId = bookDto.CategoryId;

        await _context.SaveChangesAsync();
        
        return Ok(existingBook);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _context.Categories.ToListAsync();
        return Ok(categories);
    }

    [HttpGet("categories/{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Books)
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (category == null)
        {
            _logger.LogWarning($"Category with ID {id} not found");
            return NotFound();
        }

        return Ok(category);
    }

    [HttpPost("categories/add")]
    public async Task<IActionResult> AddCategory([FromBody] CategoryDto categoryDto)
    {
        if (string.IsNullOrEmpty(categoryDto.Name))
        {
            return BadRequest("Category name is required");
        }

        var category = new Category
        {
            Name = categoryDto.Name,
            Description = categoryDto.Description
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryId }, category);
    }

    [HttpPut("categories/edit/{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        category.Name = categoryDto.Name;
        category.Description = categoryDto.Description;

        await _context.SaveChangesAsync();
        return Ok(category);
    }

    [HttpDelete("categories/del/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
