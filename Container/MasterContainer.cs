using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductAPIVS.Container;
using SalesOrderAPI.Models;

public class MasterContainer : IMasterContainer
{
    public readonly SalesDbContext _salesDbContext;
    private readonly IMapper _mapper;
    public MasterContainer(SalesDbContext salesDbContext, IMapper mapper)
    {
        _salesDbContext = salesDbContext;
        _mapper = mapper;
    }

    public async Task<List<VariantEntity>> GetAll(string variantType)
    {
        var customerData = await _salesDbContext.TblMastervariants.Where(item => item.VariantType == variantType).ToListAsync();
        if (customerData != null && customerData.Count > 0)
        {
            // using AutoMapper
            return _mapper.Map<List<TblMastervariant>, List<VariantEntity>>(customerData);
        }
        return new List<VariantEntity>();
    }
}