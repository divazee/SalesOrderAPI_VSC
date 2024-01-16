public interface IInvoiceContainer
{
    Task<List<InvoiceHeader>> GetAllInvoiceHeader();
    Task<InvoiceHeader> GetAllInvoiceHeaderByCode(string invoiceno);
    Task<List<InvoiceDetail>> GetAllInvoiceDetailByCode(string invoiceno);
    Task<ResponseType> Save(InvoiceEntity invoiceEntity);
    Task<ResponseType> Remove(string invoiceno);
}