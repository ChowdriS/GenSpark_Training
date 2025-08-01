using System;
using shop_api.Interfaces;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Services;

public class ModelService : IModelService
{
    private readonly IRepository<int, Model> _modelRepository;

    public ModelService(IRepository<int, Model> modelRepository)
    {
        _modelRepository = modelRepository;
    }

    public async Task<IEnumerable<Model>> GetAll()
    {
        return await _modelRepository.GetAll();
    }

    public async Task<Model> GetById(int id)
    {
        var model = await _modelRepository.GetById(id);
        if (model == null)
            throw new Exception("Model not found");
        return model;
    }

    public async Task<Model> Create(ModelRequestDTO modelDto)
    {
        if (modelDto == null)
            throw new Exception("ModelRequestDTO cannot be null.");

        if (string.IsNullOrWhiteSpace(modelDto.ModelName))
            throw new Exception("Model name is required.");

        var newModel = new Model
        {
            Model1 = modelDto.ModelName.Trim()
        };

        await _modelRepository.Add(newModel);
        return newModel;
    }

    public async Task<Model> UpdateAsync(int id, ModelRequestDTO modelDto)
    {
        if (modelDto == null)
            throw new Exception("ModelRequestDTO cannot be null.");

        if (string.IsNullOrWhiteSpace(modelDto.ModelName))
            throw new Exception("Model name is required.");

        var existingModel = await _modelRepository.GetById(id);
        if (existingModel == null)
            throw new Exception("Model not found");

        existingModel.Model1 = modelDto.ModelName.Trim();

        await _modelRepository.Update(id, existingModel);
        return existingModel;
    }

    public async Task<Model> DeleteAsync(int id)
    {
        var existingModel = await _modelRepository.GetById(id);
        if (existingModel == null)
            throw new Exception("Model not found");

        await _modelRepository.Delete(id);
        return existingModel;
    }
}