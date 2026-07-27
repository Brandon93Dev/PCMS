using Microsoft.AspNetCore.Mvc;
using PCMS_Backend.DTOs;
using PCMS_Backend.Extensions;
using PCMS_Backend.Models;
using PCMS_Backend.Services;

namespace PCMS_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _categoryEngine;

    public CategoriesController(CategoryService categoryEngine)
    {
        _categoryEngine = categoryEngine;
    }






    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CategoryDto dto)
    {
        if (!ValidateCategoriesModel(dto))
            return BadRequest("DTO misformed");

        var exists = await _categoryEngine.ExistsAsync(
            dto.Name, 
            dto.ParentCategoryId);

        if (exists)        
            return Conflict(new { message = "Category already exists under this parent." });
        

        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            ParentCategoryId = dto.ParentCategoryId
        };

        await _categoryEngine.AddAsync(category);

        Console.WriteLine($"Repo count: {_categoryEngine.GetTreeAsync}");
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, dto);
    }






    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var category = await _categoryEngine.GetByIdAsync(id);

        //short cirvuit and return 404
        if (category is null) return NotFound();

        var dto = new CategoryDto(
            category.Id, 
            category.Name ?? string.Empty, 
            category.Description ?? string.Empty,
            category.ParentCategoryId
        );

        return Ok(dto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var categories = await _categoryEngine.GetAllAsync();

        var dtoResults = categories.Select(c => new CategoryDto(
            c.Id, 
            c.Name ?? string.Empty, 
            c.Description ?? string.Empty,
            c.ParentCategoryId
        ));

        return Ok(dtoResults);
    }






    [HttpGet("tree")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetTree()
    {
        var categories = await _categoryEngine.GetTreeAsync();

        var dtovalues = categories.Select(c => new 
            CategoryExtensions()
            .MapToDto(c)
        );

        return Ok(dtovalues);
    }






    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] CategoryDto dto)
    {
        var existing = await _categoryEngine.GetByIdAsync(id);

        //short circuit and return 404
        if (existing is null) return NotFound();

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.ParentCategoryId = dto.ParentCategoryId;

        await _categoryEngine.UpdateAsync(existing);
        return NoContent();
    }






    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existing = await _categoryEngine.GetByIdAsync(id);
        if (existing is null) return NotFound();

        await _categoryEngine.DeleteAsync(id);
        return NoContent();
    }


    private static bool ValidateCategoriesModel(CategoryDto dto)
    {
        switch (dto)
        {           
            case { Name: null or "" }:
                return false;            
            default:
                return true;
        }
    }
}