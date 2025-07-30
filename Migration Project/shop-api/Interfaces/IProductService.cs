using System;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAll();
    Task<PaginatedResult<Product>> GetPaged(int page, int size, int? categoryId);
    Task<Product> GetById(int id);
    Task<Product> Create(ProductRequestDTO product);
    Task<Product> Update(int id, ProductRequestDTO product);
    Task<Product> Delete(int id);
}
