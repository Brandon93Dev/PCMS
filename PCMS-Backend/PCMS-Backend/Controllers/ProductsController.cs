using Microsoft.AspNetCore.Mvc;
using PCMS_Backend.DTOs;
using PCMS_Backend.Models;
using PCMS_Backend.Queries;
using PCMS_Backend.Services;
using System.Text.Json;

namespace PCMS_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {   
        _productService = productService;
    }






    [HttpGet("CustomJson")]
    public async Task<IActionResult> GetAllToCustomJson([FromQuery] ProductQuery query)
    {
        if (!ValidateQuery(query))
            return BadRequest("Query param mismatch.");

        var products = await _productService.SearchAsync(query);

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(products, jsonOptions);

        return Content(json, "application/json");
    }







    //for the requirement to perform manual modal binding
    [HttpGet("ManualLookup")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> ManualLookup()
    {
        // pull query params and do a manual lookup
        var name = HttpContext.Request.Query["name"].ToString();
        var categoryId = HttpContext.Request.Query["category"].ToString();

        int? category = null;
        if(int.TryParse(categoryId, out var parsedCategoryId))        
            category = parsedCategoryId;

        //maunally build the query
        var query = new ProductQuery
        {
            Name = string.IsNullOrWhiteSpace(name) ? string.Empty : name,
            CategoryId = category,
            //Defalt to first page and resul;t set of 10 items
            Page = 1,
            PageSize = 10
        };

        var products = await _productService.SearchAsync(query);

        var dtoResults = products.Select(p =>
            new ProductDto(
                Id: p.Id,
                Name: p.Name ?? string.Empty,
                Description: p.Description ?? string.Empty,
                SKU: p.SKU ?? string.Empty,
                Price: p.Price,
                Quantity: p.Quantity,
                CategoryId: p.CategoryId,
                CreatedAt: p.CreatedAt,
                ModifiedAt: p.ModifiedAt
            )
        );

        return Ok(dtoResults);        
    }






    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll([FromQuery] ProductQuery query)
    {
        if (!ValidateQuery(query))
            return BadRequest("Invalid query parameters.");

        //in the case where query params are provided the fuzzy search is triggered
        var products = await _productService.SearchAsync(query);

        var dtoResults = products.Select(p => new ProductDto(
            Id: p.Id,
            Name: p.Name ?? string.Empty,
            Description: p.Description ?? string.Empty,
            SKU: p.SKU,
            Price: p.Price,
            Quantity: p.Quantity,
            CategoryId: p.CategoryId,
            CreatedAt: p.CreatedAt,
            ModifiedAt: p.ModifiedAt
        ));

        return Ok(dtoResults);
    }






    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ProductDto dto)
    {
        if (!ValidateProductUpsert(dto))
            return BadRequest("Invalid product data.");

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            SKU = dto.SKU,
            Price = dto.Price,
            Quantity = dto.Quantity,
            CategoryId = dto.CategoryId,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

        await _productService.AddAsync(product);

        //Return the created product with its Id
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, dto);        
    }






    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        //short circuit and return 404
        if (product == null) 
            return NotFound();
        
        var dto = new ProductDto(
            Id: product.Id,
            Name: product.Name ?? string.Empty,
            Description: product.Description ?? string.Empty,
            SKU: product.SKU ?? string.Empty,
            Price: product.Price,
            Quantity: product.Quantity,
            CategoryId: product.CategoryId,
            CreatedAt: product.CreatedAt,   
            ModifiedAt: product.ModifiedAt
        );

        return Ok(dto);
    }






    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] ProductDto dto)
    {
        if (!ValidateProductUpsert(dto))
            return BadRequest("Invalid product data.");

        var existingProduct = await _productService.GetByIdAsync(id);

        //short circuit and return 404
        if (existingProduct == null)
            return NotFound();

        existingProduct.Name = dto.Name;
        existingProduct.Description = dto.Description;
        existingProduct.SKU = dto.SKU;
        existingProduct.Price = dto.Price;
        existingProduct.Quantity = dto.Quantity;
        existingProduct.CategoryId = dto.CategoryId;
        existingProduct.ModifiedAt = DateTime.Now;

        await _productService.UpdateAsync(existingProduct);
        return NoContent();
    }






    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existing = await _productService.GetByIdAsync(id);
        if (existing is null) return NotFound();

        await _productService.DeleteAsync(id);
        return NoContent();
    }






    #region Helpers
    private static bool ValidateQuery(ProductQuery query)
    {       
        switch (query)
        {
            case { Page: <= 0 }:
                return false;

            case { PageSize : <= 0 }:
                return false;

            case { CategoryId: < 0 }:
                return false;
            default:
                return true;
        }
    }

    private static bool ValidateProductUpsert(ProductDto dto)
    {
        switch (dto)
        { 
            case { Price: <= 0 }:
                return false;
            case { Quantity: < 0 }:
                return false;
            case { Name: null or "" }:
                return false;
            default:
                return true;
        }
    }
    #endregion
}
