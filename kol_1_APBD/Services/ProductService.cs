using kol_1_APBD.Models;
using kol_1_APBD.Repositories;

namespace kol_1_APBD.Services;

public interface IProductService
{
    public Task<Product> getOrderByIdAsync(int id);
}

public class ProductService : IProductService
{
    private IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> getOrderByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}