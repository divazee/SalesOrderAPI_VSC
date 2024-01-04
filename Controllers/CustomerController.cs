using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPIVS.Container;
using SalesOrderAPI.Models;

namespace SalesOrderAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{

    public readonly ICustomerContainer _customerContainer;

    public CustomerController(ICustomerContainer customerContainer)
    {
        _customerContainer = customerContainer;
    }

    [HttpGet("GetAll")]
    public async Task<List<CustomerEntity>> GetAll()
    {
        return await _customerContainer.GetAllCustomer();
    }
}
