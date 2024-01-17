using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPIVS.Container;
using SalesOrderAPI.Models;
// using Serilog;

namespace SalesOrderAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerContainer _customerContainer;
    private readonly ILogger<CustomerContainer> _logger;

    public CustomerController(ICustomerContainer customerContainer, ILogger<CustomerContainer> logger)
    {
        _customerContainer = customerContainer;
        _logger = logger;
    }

    [HttpGet("GetAll")]
    public async Task<List<CustomerEntity>> GetAll()
    {
        // Log.Information("Customer GetAll() triggered...");
        _logger.LogInformation("|Log ||Testing...");
        return await _customerContainer.GetAllCustomer();
    }

    [HttpGet("GetByCode")]
    public async Task<CustomerEntity> GetByCode(string code)
    {
        return await _customerContainer.GetByCode(code);
    }
}
