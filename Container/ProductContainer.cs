
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesOrderAPI.Models;

public class ProductContainer : IProductContainer
{
    public readonly SalesDbContext _salesDbContext;
    private readonly IMapper _mapper;
    public ProductContainer(SalesDbContext salesDbContext, IMapper mapper)
    {
        _salesDbContext = salesDbContext;
        _mapper = mapper;
    }

    public async Task<List<ProductEntity>> GetAll()
    {
        var productData = await _salesDbContext.TblProducts.ToListAsync();
        if (productData != null && productData.Count > 0)
        {
            return _mapper.Map<List<TblProduct>, List<ProductEntity>>(productData);
        }
        return new List<ProductEntity>();
    }

    public async Task<ProductEntity> GetByCode(string code)
    {
        var productData = await _salesDbContext.TblProducts.FirstOrDefaultAsync(item => item.Code == code);
        if (productData != null)
        {
            var _prodData = _mapper.Map<TblProduct, ProductEntity>(productData);
            if (_prodData != null)
            {
                _prodData.Variants = GetVariantByProduct(code).Result;
            }
            return _prodData;
        }
        return new ProductEntity();
    }

    public async Task<List<ProductVariantEntity>> GetVariantByProduct(string productCode)
    {
        var productData = await _salesDbContext.TblProductvariants.Where(item => item.ProductCode == productCode).ToListAsync();
        if (productData != null && productData.Count > 0)
        {
            return _mapper.Map<List<TblProductvariant>, List<ProductVariantEntity>>(productData);
        }
        return new List<ProductVariantEntity>();
    }

    public async Task<List<ProductEntity>> GetByCategory(int category)
    {
        var productData = await _salesDbContext.TblProducts.Where(item => item.Category == category).ToListAsync();
        if (productData != null)
        {
            return _mapper.Map<List<TblProduct>, List<ProductEntity>>(productData);
        }
        return new List<ProductEntity>();
    }

    public async Task<ResponseType> SaveProduct(ProductEntity product)
    {
        try
        {
            string result = string.Empty;
            int processCount = 0;

            if (product != null)
            {
                using var dbTransaction = await _salesDbContext.Database.BeginTransactionAsync();
                // check existing product
                var _product = await _salesDbContext.TblProducts.FirstOrDefaultAsync(item => item.Code == product.Code);
                if (_product != null)
                {
                    // update here
                    _product.Name = product.Name;
                    _product.Category = product.Category;
                    _product.Price = product.Price;
                    _product.Remarks = product.Remarks;
                    // await _salesDbContext.SaveChangesAsync();
                }
                else
                {
                    // create new record
                    var _newproduct = new TblProduct()
                    {
                        Code = product.Code,
                        Name = product.Name,
                        Category = product.Category,
                        Price = product.Price,
                        Remarks = product.Remarks
                    };
                    await _salesDbContext.TblProducts.AddAsync(_newproduct);
                    // await _salesDbContext.SaveChangesAsync();
                }
                if (product.Variants != null && product.Variants.Count > 0)
                {
                    product.Variants.ForEach(item =>
                    {
                        var _resp = SaveProductVariant(item, product.Code);
                        if (_resp.Result)
                        {
                            processCount++;
                        }
                    });

                    if (processCount == product.Variants.Count)
                    {
                        await _salesDbContext.SaveChangesAsync();
                        await dbTransaction.CommitAsync();
                        return new ResponseType() { Result = "pass", KeyValue = product.Code };
                    }
                    else
                    {
                        await dbTransaction.RollbackAsync();
                    }
                }
            }
            else
            {

            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Exception in SaveProduct: {ex.Message}");
            // throw;
        }
        return new ResponseType() { Result = "fail", KeyValue = string.Empty };
    }

    public async Task<bool> SaveProductVariant(ProductVariantEntity _variant, string ProductCode)
    {
        bool result = false;
        try
        {
            var _existData = await _salesDbContext.TblProductvariants.FirstOrDefaultAsync(item => item.Id == _variant.Id);
            if (_existData != null)
            {
                _existData.ColorId = _variant.ColorId;
                _existData.SizeId = _variant.SizeId;
                _existData.Price = _variant.Price;
                _existData.ProductCode = _variant.ProductCode;
                _existData.Remarks = _variant.Remarks;
            }
            else
            {
                var _newrecord = new TblProductvariant()
                {
                    ColorId = _variant.ColorId,
                    SizeId = _variant.SizeId,
                    Price = _variant.Price,
                    ProductCode = ProductCode,
                    Remarks = _variant.Remarks,
                    IsActive = true
                };
                await _salesDbContext.TblProductvariants.AddAsync(_newrecord);
            }
            result = true;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Exception in SaveProductVariant: {ex.Message}");
            // throw;
        }
        return result;
    }
}