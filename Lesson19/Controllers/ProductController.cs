using Lesson19.Data;
using Lesson19.Data.EF;
using Lesson19.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lesson19.Controllers;

[Route("[controller]")]
public class ProductController : ControllerBase
{
    public readonly IDataContext context = new EfDataContext();

    [HttpGet("{id}")]
    public async Task<ProductModel?> GetProduct([FromRoute]int id)
    {
        var dbProduct =  (await context.SelectProducts()).FirstOrDefault(product => product.Id == id);

        ProductModel product = dbProduct.ProductType switch {
            Data.Models.ProductType.Accessories => new AccessoriesModel(),
            Data.Models.ProductType.Book => new BookModel(),
            Data.Models.ProductType.Food => new FoodModel()
        };

        product.Id = product.Id;
        product.Name = product.Name;
        product.Description = product.Description;
        product.Price = product.Price;

        return product;
    }
    
    [HttpPost("{id}")]
    public async Task<ProductModel?> UpdateProduct([FromRoute]int id, [FromBody] ProductModel updatedProduct)
    {
        updatedProduct.Id = id;
        await context.UpdateProduct(new Data.Models.Product() {
            Id = updatedProduct.Id,
            Name = updatedProduct.Name,
            Description = updatedProduct.Description,
            Price = updatedProduct.Price,
            ProductType = updatedProduct.ProductType
        });
        return updatedProduct;
    }

    [HttpPost("create-product")]
    public async Task<ProductModel?> CreateProduct([FromBody] ProductModel createdProduct) {
        await context.InsertProduct(new Data.Models.Product() {
            Id = createdProduct.Id,
            Name = createdProduct.Name,
            Description = createdProduct.Description,
            Price = createdProduct.Price,
            ProductType = createdProduct.ProductType
        });
        return createdProduct;
    }
}