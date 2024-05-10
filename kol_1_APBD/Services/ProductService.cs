using kol_1_APBD.Models;
using kol_1_APBD.Repositories;

namespace kol_1_APBD.Services;

public interface IProductService
{
    public Task<Order> getOrderByIdAsync(int id);
    public Task<bool> deleteProductById(int id);
}

public class ProductService : IProductService
{
    private IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Order> getOrderByIdAsync(int id)
    {
        return await _productRepository.getOrderByIdAsync(id);
    }

    public async Task<bool> deleteProductById(int id)
    {
        return await _productRepository.deleteByIdAsync(id);
    }
    
}