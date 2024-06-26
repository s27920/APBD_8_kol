﻿using System.Data;
using kol_1_APBD.Exceptions;
using kol_1_APBD.Services;
using Microsoft.AspNetCore.Mvc;

namespace kol_1_APBD.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductController : ControllerBase
{
    private IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> getProductsByOrderIdAsync(int id)
    {
        try
        {
            return Ok(await _productService.getOrderByIdAsync(id));
        }
        catch (NotFoundException e)
        {
            return NotFound(e);
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> deleteProductByIdAsync(int id)
    {
        try
        {
            return Ok(await _productService.deleteProductByIdAsync(id));
        }
        catch (ConflictException e)
        {
            return Conflict(e);
        }
        catch (NotFoundException e)
        {
            return NotFound(e);
        }
        
    }
}