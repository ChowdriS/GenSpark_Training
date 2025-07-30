using System;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Interfaces;

public interface ICategoryService
{
    public Task<PaginatedResult<Category>> GetCategoryList(int pageNumber, int pageSize);

    public Task<Category> AddCategory(CategoryRequestDTO category);

    public Task<Category> UpdateCategory(int id, CategoryRequestDTO category);

    public Task<Category> GetCategoryById(int id);

    public Task<Category> DeleteCategoryById(int id);
}
