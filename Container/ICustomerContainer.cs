
using SalesOrderAPI.Models;

namespace ProductAPIVS.Container;

public interface ICustomerContainer
{
    Task<List<CustomerEntity>> GetAllCustomer();
    Task<CustomerEntity> GetByCode(string code);
}