
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
            return _mapper.Map<TblProduct, ProductEntity>(productData);
        }
        return new ProductEntity();
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
}