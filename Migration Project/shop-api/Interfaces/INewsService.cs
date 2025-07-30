using System;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Interfaces;

public interface INewsService
{
    public Task<IEnumerable<News>> GetAll();
    public Task<News> GetById(int id);
    public Task<News> Create(NewsRequestDTO dto);
    public Task<News> Update(int id, NewsUpdateRequestDTO dto);
    public Task<News> Delete(int id);
    public Task<string> ExportToCsv();
    public Task<string> ExportToExcel();
}
