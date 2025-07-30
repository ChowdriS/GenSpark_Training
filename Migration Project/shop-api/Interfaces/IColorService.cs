using System;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Interfaces;

public interface IColorService
{
    public Task<IEnumerable<Color>> GetAll();
    

    public Task<Color> GetById(int id);
    

    public Task<Color> Create(ColorRequestDTO color);
    

    public Task<Color> UpdateAsync(int id, ColorRequestDTO dto);
    

    public Task<Color> DeleteAsync(int id);
    
}
