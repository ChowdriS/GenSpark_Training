using System;
using shop_api.Models;
using shop_api.Models.DTO;
using System.Text;
using shop_api.Context;
using Microsoft.EntityFrameworkCore;
using shop_api.Interfaces;

namespace shop_api.Services;

public class NewsService : INewsService
{
    private readonly IRepository<int, News> _newsRepository;
    private readonly ShopContext _context;

    public NewsService(IRepository<int, News> newsRepository, ShopContext context)
    {
        _newsRepository = newsRepository;
        _context = context;
    }

    public async Task<IEnumerable<News>> GetAll()
    {
        return await _newsRepository.GetAll();
    }

    public async Task<News> GetById(int id)
    {
        var news = await _newsRepository.GetById(id);
        return news;
    }

    public async Task<News> Create(NewsRequestDTO dto)
    {
        if (dto == null)
            throw new Exception("NewsRequestDTO cannot be null.");

        if (dto.Title == null || dto.Title == "")
            throw new Exception("Title is required.");

        if (dto.ShortDescription == null || dto.ShortDescription == "")
            throw new Exception("ShortDescription is required.");

        if (dto.Content == null || dto.Content == "")
            throw new Exception("Content is required.");

        var news = new News
        {
            UserId = dto.UserId,
            Title = dto.Title.Trim(),
            ShortDescription = dto.ShortDescription.Trim(),
            Image = dto.Image?.Trim(),
            Content = dto.Content.Trim(),
            CreatedDate = dto.CreatedDate,
            Status = dto.Status
        };

        await _newsRepository.Add(news);

        return news;
    }

    public async Task<News> Update(int id, NewsUpdateRequestDTO dto)
    {
        if (dto == null)
            throw new Exception("Update data cannot be null.");

        var news = await _newsRepository.GetById(id);

        if (dto.Title != null)
            news.Title = dto.Title.Trim() == "" ? throw new Exception("Title cannot be empty.") : dto.Title.Trim();

        if (dto.ShortDescription != null)
            news.ShortDescription = dto.ShortDescription.Trim() == "" ? throw new Exception("ShortDescription cannot be empty.") : dto.ShortDescription.Trim();

        if (dto.Image != null)
            news.Image = dto.Image.Trim() == "" ? null : dto.Image.Trim();

        if (dto.Content != null)
            news.Content = dto.Content.Trim() == "" ? throw new Exception("Content cannot be empty.") : dto.Content.Trim();

        if (dto.Status != null)
            news.Status = dto.Status;

        await _newsRepository.Update(id, news);

        return news;
    }

    public async Task<News> Delete(int id)
    {
        var news = await _newsRepository.GetById(id);
        await _newsRepository.Delete(id);
        return news;
    }

    public async Task<string> ExportToCsv()
    {
        var newsList = await _context.News.OrderBy(x => x.NewsId).ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("\"NewsId\",\"Title\",\"ShortDescription\",\"CreatedDate\",\"Status\"");

        foreach (var news in newsList)
        {
            sb.AppendLine($"\"{news.NewsId}\",\"{EscapeCsv(news.Title)}\",\"{EscapeCsv(news.ShortDescription)}\",\"{news.CreatedDate:O}\",\"{(news.Status == 1 ? "true" : "false")}\"");
        }

        return sb.ToString();
    }

    public async Task<string> ExportToExcel()
    {
        var newsList = await _context.News.OrderBy(x => x.NewsId).ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("<table><tr><th>NewsId</th><th>Title</th><th>ShortDescription</th><th>CreatedDate</th><th>Status</th></tr>");

        foreach (var news in newsList)
        {
            sb.AppendLine($"<tr><td>{news.NewsId}</td><td>{EscapeHtml(news.Title)}</td><td>{EscapeHtml(news.ShortDescription)}</td><td>{news.CreatedDate:O}</td><td>{news.Status}</td></tr>");
        }

        sb.AppendLine("</table>");
        return sb.ToString();
    }


    private static string EscapeCsv(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        return input.Replace("\"", "\"\""); 
    }

    private static string EscapeHtml(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        return System.Net.WebUtility.HtmlEncode(input);
    }
}
