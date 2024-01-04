using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductAPIVS.Container;
using SalesOrderAPI.Models;

public class CustomerContainer : ICustomerContainer
{
    public readonly SalesDbContext _salesDbContext;
    private readonly IMapper _mapper;
    public CustomerContainer(SalesDbContext salesDbContext, IMapper mapper)
    {
        _salesDbContext = salesDbContext;
        _mapper = mapper;
    }

    public async Task<List<CustomerEntity>> GetAllCustomer()
    {
        // return await _salesDbContext.TblCustomers.ToListAsync(); //before mapping
        var customerData = await _salesDbContext.TblCustomers.ToListAsync();
        if (customerData != null && customerData.Count > 0)
        {
            // using AutoMapper
            return _mapper.Map<List<TblCustomer>, List<CustomerEntity>>(customerData);
        }
        return new List<CustomerEntity>();
    }
}