namespace BookCart
{
    public class CreateCartDto
    {
        public int UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
    }

    public class CartItemDto
    {
        public int BookId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}