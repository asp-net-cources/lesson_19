using Lesson19.Data.Models;

namespace Lesson19.Models;

public class AccessoriesModel : ProductModel
{
    public new ProductType ProductType { get; } = ProductType.Accessories;
}
