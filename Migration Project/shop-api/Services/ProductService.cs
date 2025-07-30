using System;
using Microsoft.EntityFrameworkCore;
using shop_api.Context;
using shop_api.Interfaces;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Services;

public class ProductService : IProductService
{
    private readonly IRepository<int, Product> _productRepository;
    public ProductService(IRepository<int, Product> productRepository)
    {
        _productRepository = productRepository;
    }
    private void ValidateProductRequest(ProductRequestDTO product, bool isCreate)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(product.ProductName))
            errors.Add("ProductName is required.");

        if (string.IsNullOrWhiteSpace(product.Image))
            errors.Add("Image URL is required.");

        if (!product.Price.HasValue || product.Price <= 0)
            errors.Add("Valid Price is required.");

        if (!product.UserId.HasValue)
            errors.Add("UserId is required.");

        if (!product.CategoryId.HasValue)
            errors.Add("CategoryId is required.");

        if (!product.ColorId.HasValue)
            errors.Add("ColorId is required.");

        if (!product.ModelId.HasValue)
            errors.Add("ModelId is required.");

        if (!product.StorageId.HasValue)
            errors.Add("StorageId is required.");

        if (product.SellStartDate == default)
            errors.Add("SellStartDate is required.");

        if (product.SellEndDate == default)
            errors.Add("SellEndDate is required.");

        if (!product.IsNew.HasValue)
            errors.Add("IsNew flag is required.");

        if (errors.Any())
            throw new ArgumentException(string.Join(" ", errors));
    }

    public async Task<Product> Create(ProductRequestDTO product)
    {
        ValidateProductRequest(product, isCreate: true);

        var newProduct = new Product
        {
            ProductName = product.ProductName!,
            Image = product.Image!,
            Price = product.Price!.Value,
            UserId = product.UserId!.Value,
            CategoryId = product.CategoryId!.Value,
            ColorId = product.ColorId!.Value,
            ModelId = product.ModelId!.Value,
            StorageId = product.StorageId!.Value,
            SellStartDate = product.SellStartDate,
            SellEndDate = product.SellEndDate,
            IsNew = product.IsNew!.Value
        };

        await _productRepository.Add(newProduct);
        return newProduct;
    }

    public async Task<Product> Update(int id, ProductRequestDTO product)
    {
        var existing = await _productRepository.GetById(id);
        if (existing == null)
            throw new Exception("Product not found");

        if (!string.IsNullOrWhiteSpace(product.ProductName))
            existing.ProductName = product.ProductName;

        if (!string.IsNullOrWhiteSpace(product.Image))
            existing.Image = product.Image;

        if (product.Price.HasValue)
            existing.Price = product.Price.Value;

        if (product.UserId.HasValue)
            existing.UserId = product.UserId.Value;

        if (product.CategoryId.HasValue)
            existing.CategoryId = product.CategoryId.Value;

        if (product.ColorId.HasValue)
            existing.ColorId = product.ColorId.Value;

        if (product.ModelId.HasValue)
            existing.ModelId = product.ModelId.Value;

        if (product.StorageId.HasValue)
            existing.StorageId = product.StorageId.Value;

        if (product.SellStartDate != default)
            existing.SellStartDate = product.SellStartDate;

        if (product.SellEndDate != default)
            existing.SellEndDate = product.SellEndDate;

        if (product.IsNew.HasValue)
            existing.IsNew = product.IsNew.Value;

        await _productRepository.Update(existing.ProductId,existing);
        return existing;
    }


    public async Task<Product> Delete(int id)
    {
        var product = await _productRepository.GetById(id);

        await _productRepository.Delete(id);
        return product;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _productRepository.GetAll();
    }

    public async Task<PaginatedResult<Product>> GetPaged(int page, int size, int? categoryId)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 10;

        var allProducts = await _productRepository.GetAll();

        if (categoryId.HasValue)
        {
            allProducts = allProducts.Where(p => p.CategoryId == categoryId.Value);
        }

        var totalItems = allProducts.Count();

        var totalPages = (int)Math.Ceiling(totalItems / (double)size);

        var items = allProducts
            .OrderByDescending(p => p.ProductId)
            .Skip((page - 1) * size)
            .Take(size)
            .ToList();

        return new PaginatedResult<Product>
        {
            Page = page,
            PageSize = size,
            TotalPages = totalPages,
            Items = items
        };
    }
    public async Task<Product> GetById(int id)
    {
        var product = await _productRepository.GetById(id);

        if (product == null)
            throw new Exception("Product not found");

        return product;
    }
}
