using Lesson19.Data.Models;

namespace Lesson19.Models;

public class BookModel : ProductModel
{
    public new ProductType ProductType { get; } = ProductType.Book;
}
