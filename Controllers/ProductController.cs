using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPIVS.Container;
using SalesOrderAPI.Models;

namespace SalesOrderAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{

    public readonly IProductContainer _productContainer;

    public ProductController(IProductContainer productContainer)
    {
        _productContainer = productContainer;
    }

    [HttpGet("GetAll")]
    public async Task<List<ProductEntity>> GetAll()
    {
        return await _productContainer.GetAll();
    }

    [HttpGet("GetByCode")]
    public async Task<ProductEntity> GetByCode(string code)
    {
        return await _productContainer.GetByCode(code);
    }

    [HttpGet("GetByCategory")]
    public async Task<List<ProductEntity>> GetByCategory(int category)
    {
        return await _productContainer.GetByCategory(category);
    }
}
