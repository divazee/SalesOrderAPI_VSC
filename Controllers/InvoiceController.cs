using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPIVS.Container;
using SalesOrderAPI.Models;

namespace SalesOrderAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceContainer _invoiceContainer;

    public InvoiceController(IInvoiceContainer invoiceContainer)
    {
        _invoiceContainer = invoiceContainer;
    }

    [HttpGet("GetAllHeader")]
    public async Task<List<InvoiceHeader>> GetAllHeader()
    {
        return await _invoiceContainer.GetAllInvoiceHeader();
    }

    [HttpGet("GetAllHeaderByCode")]
    public async Task<InvoiceHeader> GetAllHeaderByCode(string invoiceno)
    {
        return await _invoiceContainer.GetAllInvoiceHeaderByCode(invoiceno);
    }

    [HttpGet("GetAllDetailByCode")]
    public async Task<List<InvoiceDetail>> GetAllDetailByCode(string invoiceno)
    {
        return await _invoiceContainer.GetAllInvoiceDetailByCode(invoiceno);
    }

    [HttpPost("Save")]
    public async Task<ResponseType> Save([FromBody] InvoiceEntity invoiceEntity)
    {
        return await _invoiceContainer.Save(invoiceEntity);
    }

    [HttpDelete("Remove")]
    public async Task<ResponseType> Remove(string invoiceno)
    {
        return await _invoiceContainer.Remove(invoiceno);
    }
}
