using System.ComponentModel.DataAnnotations;

namespace BookCart
{
    public class BookCreateDto
    {
        public string? Title { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }
        public int AuthorId { get; set; }

        public int CategoryId { get; set; }
    }

    public class CategoryDto
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

    }
    
}