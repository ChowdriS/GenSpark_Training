using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shop_api.Context;
using shop_api.Interfaces;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Services;

public class ColorService : IColorService
{
    private readonly IRepository<int, Color> _colorRepository;

    public ColorService(IRepository<int, Color> colorRepository)
    {
        _colorRepository = colorRepository;
    }

    public async Task<IEnumerable<Color>> GetAll()
    {
        return await _colorRepository.GetAll();
    }

    public async Task<Color> GetById(int id)
    {
        var color = await _colorRepository.GetById(id);
        return color;
    }

    public async Task<Color> Create(ColorRequestDTO color)
    {
        if (color == null)
            throw new Exception("ColorRequestDTO cannot be null.");

        if (color.colorName == null || color.colorName.Trim() == "")
            throw new Exception("Color name is required.");

        var newColor = new Color
        {
            Color1 = color.colorName.Trim()
        };

        await _colorRepository.Add(newColor);
        return newColor;
    }

    public async Task<Color> UpdateAsync(int id, ColorRequestDTO dto)
    {
        if (dto == null)
            throw new Exception("ColorRequestDTO cannot be null.");

        if (dto.colorName == null || dto.colorName.Trim() == "")
            throw new Exception("Color name is required.");

        var existingColor = await _colorRepository.GetById(id);
        existingColor.Color1 = dto.colorName.Trim();

        await _colorRepository.Update(id, existingColor);
        return existingColor;
    }

    public async Task<Color> DeleteAsync(int id)
    {
        var existingColor = await _colorRepository.GetById(id);
        await _colorRepository.Delete(id);
        return existingColor;
    }
}
