namespace Lesson19.Data.Models;

public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public int Count { get; set; }
    public DateTime CreatedAt { get; set; }
}
