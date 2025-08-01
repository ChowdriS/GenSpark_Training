using System;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Interfaces;

public interface IModelService
{
    Task<IEnumerable<Model>> GetAll();
    Task<Model> GetById(int id);
    Task<Model> Create(ModelRequestDTO modelDto);
    Task<Model> UpdateAsync(int id, ModelRequestDTO modelDto);
    Task<Model> DeleteAsync(int id);
}