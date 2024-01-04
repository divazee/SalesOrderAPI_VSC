using Microsoft.EntityFrameworkCore;
using ProductAPIVS.Container;
using SalesOrderAPI.Models;

public class CustomerContainer : ICustomerContainer
{
    public readonly SalesDbContext _salesDbContext;
    public CustomerContainer(SalesDbContext salesDbContext)
    {
        _salesDbContext = salesDbContext;
    }

    public async Task<List<CustomerEntity>> GetAllCustomer()
    {
        // return await _salesDbContext.TblCustomers.ToListAsync(); //before mapping
        var customerData = await _salesDbContext.TblCustomers.ToListAsync();
        if (customerData != null && customerData.Count > 0)
        {

        }
        return new List<CustomerEntity>();
    }
}