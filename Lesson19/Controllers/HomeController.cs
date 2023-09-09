using System.Collections.Generic;
using System.Diagnostics;
using Lesson19.Data;
using Lesson19.Data.EF;
using Lesson19.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lesson19.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public IDataContext _dataContext;

    public HomeController(ILogger<HomeController> logger, IDataContext dataContext)
    {
        _logger = logger;
        _dataContext = dataContext;
    }

    public async Task<IActionResult> Index(string firstname, string lastname, string gender)
    {
        var model = new IndexModel {
            Products = (await _dataContext.SelectProducts()).Where(product => product != null).Cast<ProductModel>().ToList()
        };
        return View(model);
    }

    [HttpPost("create-product")]
    public async Task<IActionResult> CreateProduct([FromForm]ProductModel newProduct)
    {
        await _dataContext.InsertProduct(new Data.Models.Product() {
            Id = newProduct.Id,
            Name = newProduct.Name,
            Description = newProduct.Description,
            Price = newProduct.Price,
            ProductType = newProduct.ProductType
        });
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}