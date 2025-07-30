using System;
using Microsoft.EntityFrameworkCore;
using shop_api.Context;
using shop_api.Interfaces;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Services;
public class CategoryService : ICategoryService
{
    private readonly IRepository<int, Category> _categoryRepository;

    public CategoryService(IRepository<int, Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<PaginatedResult<Category>> GetCategoryList(int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var allCategories = await _categoryRepository.GetAll();
        var totalItems = allCategories.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var items = allCategories
            .OrderBy(c => c.CategoryId);

        var val = items.Skip((pageNumber - 1) * pageSize).ToList();

        var data = val.Take(pageSize).ToList();

        return new PaginatedResult<Category>
        {
            Page = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            Items = data
        };
    }


    public async Task<Category> AddCategory(CategoryRequestDTO category)
    {
        if (category == null)
            throw new Exception("CategoryRequestDTO cannot be null.");
        if (category.CategoryName == null || category.CategoryName.Trim() == "")
            throw new Exception("Category name is required.");
        var newCategory = new Category
        {
            Name = category.CategoryName.Trim()
        };
        await _categoryRepository.Add(newCategory);
        return newCategory;
    }

    public async Task<Category> UpdateCategory(int id, CategoryRequestDTO category)
    {
        if (category == null)
            throw new Exception("CategoryRequestDTO cannot be null.");
        if (category.CategoryName == null || category.CategoryName.Trim() == "")
            throw new Exception("Category name is required.");
        var existingCategory = await _categoryRepository.GetById(id);
        existingCategory.Name = category.CategoryName.Trim();
        await _categoryRepository.Update(id, existingCategory);
        return existingCategory;
    }

    public async Task<Category> GetCategoryById(int id)
    {
        var category = await _categoryRepository.GetById(id);
        return category;
    }

    public async Task<Category> DeleteCategoryById(int id)
    {
        var category = await _categoryRepository.GetById(id);
        await _categoryRepository.Delete(id);
        return category;
    }

}
