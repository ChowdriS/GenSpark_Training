using FileApp.Models;

namespace FileApp.Interfaces;

public interface IFileService
{
    public Task<FileModel> UploadFile(IFormFile file, string userName);
    public Task<IEnumerable<object>> GetAll();
    public Task<FileModel> GetFile(int id);
}