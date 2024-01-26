public interface IProductContainer
{

    Task<List<ProductEntity>> GetAll();
    Task<ProductEntity> GetByCode(string code);
    Task<List<ProductEntity>> GetByCategory(int category);
    Task<ResponseType> SaveProduct(ProductEntity product);
}