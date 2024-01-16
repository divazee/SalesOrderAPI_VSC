using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesOrderAPI.Models;

public class InvoiceContainer : IInvoiceContainer
{
    private readonly SalesDbContext _salesDbContext;
    private readonly IMapper _mapper;
    public InvoiceContainer(SalesDbContext salesDbContext, IMapper mapper)
    {
        _salesDbContext = salesDbContext;
        _mapper = mapper;
    }

    public async Task<List<InvoiceHeader>> GetAllInvoiceHeader()
    {
        var _data = await _salesDbContext.TblSalesHeaders.ToListAsync();
        if (_data != null && _data.Count > 0)
        {
            return _mapper.Map<List<TblSalesHeader>, List<InvoiceHeader>>(_data);
        }
        return new List<InvoiceHeader>();
    }

    public async Task<InvoiceHeader> GetAllInvoiceHeaderByCode(string invoiceno)
    {
        var _data = await _salesDbContext.TblSalesHeaders.FirstOrDefaultAsync(item => item.InvoiceNo == invoiceno);
        if (_data != null)
        {
            return _mapper.Map<TblSalesHeader, InvoiceHeader>(_data);
        }
        return new InvoiceHeader();
    }

    public async Task<List<InvoiceDetail>> GetAllInvoiceDetailByCode(string invoiceno)
    {
        var _data = await _salesDbContext.TblSalesProductInfos.Where(item => item.InvoiceNo == invoiceno).ToListAsync();
        if (_data != null && _data.Count > 0)
        {
            return _mapper.Map<List<TblSalesProductInfo>, List<InvoiceDetail>>(_data);
        }
        return new List<InvoiceDetail>();
    }

    public async Task<ResponseType> Save(InvoiceEntity invoiceEntity)
    {
        string result = string.Empty;
        int processCount = 0;
        var response = new ResponseType();

        if (invoiceEntity != null)
        {
            using (var dbTransaction = await _salesDbContext.Database.BeginTransactionAsync())
            {
                if (invoiceEntity.header != null)
                    result = await SaveHeader(invoiceEntity.header);

                if (!string.IsNullOrEmpty(result) && invoiceEntity.details != null && invoiceEntity.details.Count > 0)
                {
                    invoiceEntity.details.ForEach(item =>
                    {
                        bool saveResult = SaveDetail(item, invoiceEntity.header.CreateUser).Result;
                        if (saveResult) { processCount++; }
                    });

                    if (invoiceEntity.details.Count == processCount)
                    {
                        await _salesDbContext.SaveChangesAsync();
                        await dbTransaction.CommitAsync();
                        response.Result = "pass";
                        response.KeyValue = result;
                    }
                    else
                    {
                        await dbTransaction.RollbackAsync();
                        response.Result = "fail";
                        response.KeyValue = "here";
                        // response.KeyValue = string.Empty;
                    }
                }
                else
                {
                    response.Result = "fail";
                    response.KeyValue = result + "..string.Empty";
                }
            }
        }
        else
        {
            return new ResponseType();
        }
        return response;
    }

    private async Task<string> SaveHeader(InvoiceHeader invoiceHeader)
    {
        string results = string.Empty;

        try
        {
            TblSalesHeader _header = _mapper.Map<InvoiceHeader, TblSalesHeader>(invoiceHeader);
            var header = await _salesDbContext.TblSalesHeaders.FirstOrDefaultAsync(item => item.InvoiceNo == invoiceHeader.InvoiceNo);

            if (header != null)
            {
                // update existing header 
                header.CustomerId = invoiceHeader.CustomerId;
                header.CustomerName = invoiceHeader.CustomerName;
                header.DeliveryAddress = invoiceHeader.DeliveryAddress;
                header.Total = invoiceHeader.Total;
                header.Remarks = invoiceHeader.Remarks;
                header.Tax = invoiceHeader.Tax;
                header.NetTotal = invoiceHeader.NetTotal;
                header.ModifyUser = invoiceHeader.CreateUser;
                header.ModifyDate = DateTime.Now;

                var _data = await _salesDbContext.TblSalesProductInfos
                                .Where(item => item.InvoiceNo == invoiceHeader.InvoiceNo)
                                .ToListAsync();
                if (_data != null && _data.Count > 0)
                {
                    _salesDbContext.TblSalesProductInfos.RemoveRange(_data);
                }
            }
            else
            {
                // create new header
                await _salesDbContext.TblSalesHeaders.AddAsync(_header);
            }
            results = invoiceHeader.InvoiceNo;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return results;
    }

    private async Task<bool> SaveDetail(InvoiceDetail invoiceDetail, string User)
    {
        try
        {
            TblSalesProductInfo _detail = _mapper.Map<InvoiceDetail, TblSalesProductInfo>(invoiceDetail);
            _detail.CreateDate = DateTime.Now;
            _detail.CreateUser = User;
            await _salesDbContext.TblSalesProductInfos.AddAsync(_detail);
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public async Task<ResponseType> Remove(string invoiceno)
    {
        try
        {
            using var dbTransaction = await _salesDbContext.Database.BeginTransactionAsync();
            var _data = await _salesDbContext.TblSalesHeaders.FirstOrDefaultAsync(item => item.InvoiceNo == invoiceno);
            if (_data != null)
            {
                _salesDbContext.TblSalesHeaders.Remove(_data);
            }

            var _delData = await _salesDbContext.TblSalesProductInfos.Where(item => item.InvoiceNo == invoiceno).ToListAsync();
            if (_delData != null && _delData.Count > 0)
            {
                _salesDbContext.TblSalesProductInfos.RemoveRange(_delData);
            }
            await _salesDbContext.SaveChangesAsync();
            await dbTransaction.CommitAsync();
            return new ResponseType() { Result = "pass", KeyValue = invoiceno };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}