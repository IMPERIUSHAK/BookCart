namespace BookCart.Models;
using Microsoft.EntityFrameworkCore;
public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
