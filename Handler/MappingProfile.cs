using AutoMapper;
using SalesOrderAPI.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TblCustomer, CustomerEntity>()
            .ForMember(item => item.StatusName, item => item.MapFrom(item => item.IsActive == true ? "Active" : "InActive"));

        CreateMap<TblSalesHeader, InvoiceHeader>().ReverseMap();
        CreateMap<TblSalesProductInfo, InvoiceDetail>().ReverseMap();
        CreateMap<TblProduct, ProductEntity>().ReverseMap();
    }
}