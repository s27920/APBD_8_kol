﻿using kol_1_APBD.Exceptions;
using kol_1_APBD.Models;
using kol_1_APBD.Repositories;

namespace kol_1_APBD.Services;

public interface IProductService
{
    public Task<Order> getOrderByIdAsync(int id);
    public Task<bool> deleteProductByIdAsync(int id);
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
        if (!await _productRepository.checkIfOrderExistsASync(id))
        {
            throw new NotFoundException("No Order found under given id");
        }
        return await _productRepository.getOrderByIdAsync(id);
    }

    public async Task<bool> deleteProductByIdAsync(int id)
    {
        if (!await _productRepository.checkIfOrderExistsASync(id))
        {
            throw new NotFoundException("No Order found under given id");
        }
        return await _productRepository.deleteByIdAsync(id);
    }
    
}