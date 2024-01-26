using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPIVS.Container;
using SalesOrderAPI.Models;

namespace SalesOrderAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductContainer _productContainer;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(IProductContainer productContainer, IWebHostEnvironment webHostEnvironment)
    {
        _productContainer = productContainer;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet("GetAll")]
    public async Task<List<ProductEntity>> GetAll()
    {
        var productList = await _productContainer.GetAll();
        if (productList != null && productList.Count > 0)
        {
            productList.ForEach(item =>
            {
                item.ProductImage = GetImageByProduct(item.Code);
            });
        }
        else
        {
            return new List<ProductEntity>();
        }
        return productList;
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

    [HttpPost("UploadImage")]
    public async Task<ActionResult> UploadImage()
    {
        bool result = false;
        try
        {
            var _uploadedFiles = Request.Form.Files;
            foreach (IFormFile file in _uploadedFiles)
            {
                string fileName = file.FileName; //productCode
                string filePath = GetFilePath(fileName);

                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }

                string imagePath = filePath + "\\image.png";

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                using (FileStream stream = System.IO.File.Create(imagePath))
                {
                    await file.CopyToAsync(stream);
                    result = true;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
        return Ok(result);

    }

    [HttpGet("RemoveImage/{code}")]
    public ResponseType RemoveImage(string code)
    {
        string
            filePath = GetFilePath(code),
            imagePath = filePath + "/image.png";
        try
        {
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            return new ResponseType { Result = "pass", KeyValue = code };
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    [HttpPost("SaveProduct")]
    public async Task<ResponseType> SaveProduct([FromBody] ProductEntity _product)
    {
        return await _productContainer.SaveProduct(_product);
    }

    [NonAction]
    private string GetFilePath(string productCode)
    {
        return _webHostEnvironment.WebRootPath + "/Uploads/Product/" + productCode;
    }

    private string GetImageByProduct(string productCode)
    {
        string
            imageUrl = string.Empty,
            // hostUrl = "http://localhost:5217/",
            hostUrl = $"{Request.Scheme}://{Request.Host.Host}:{Request.Host.Port}/",
            filePath = GetFilePath(productCode),
            imagePath = filePath + "/image.png";

        if (!System.IO.File.Exists(imagePath))
        {
            imageUrl = hostUrl + "Uploads/common/noimage.png";
        }
        else
        {
            imageUrl = hostUrl + "Uploads/Product/" + productCode + "/image.png";
        }

        return imageUrl;
    }
}
