using Lesson19.Data.Models;

namespace Lesson19.Models;

public class FoodModel : ProductModel
{
    public new ProductType ProductType { get; } = ProductType.Food;
}
