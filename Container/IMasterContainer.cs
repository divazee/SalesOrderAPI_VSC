
namespace ProductAPIVS.Container;

public interface IMasterContainer
{
    Task<List<VariantEntity>> GetAll(string variantType);
}