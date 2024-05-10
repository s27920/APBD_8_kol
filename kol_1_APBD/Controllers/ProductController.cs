using System.Data;
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

    [HttpPost("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> getProductsByOrderIdAsync(int id)
    {
        return Ok(await _productService.getOrderByIdAsync(id));
    }
}