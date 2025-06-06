using FileApp.Interfaces;
using FileApp.Models;
using FileApp.Exceptions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FileApp.Services;

public class FileService : IFileService
{
    private readonly IRepository<int, FileModel> _fileRepository;
    private readonly IRepository<string, User> _userRepository;
    public FileService(IRepository<int, FileModel> fileRepository,
                        IRepository<string, User> userRepository)
    {
        _fileRepository = fileRepository;
        _userRepository = userRepository;
    }

    public async Task<FileModel> UploadFile(IFormFile file, string username)
    {
        try
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be null or empty.");
            
            string fileSize;
            if (file.Length >= 1024 * 1024 * 1024)
                fileSize = $"{file.Length / (1024.0 * 1024 * 1024):F2} GB";
            else if (file.Length >= 1024 * 1024)
                fileSize = $"{file.Length / (1024.0 * 1024):F2} MB";
            else if (file.Length >= 1024)
                fileSize = $"{file.Length / 1024.0:F2} KB";
            else
                fileSize = $"{file.Length} bytes";
            string fileType = System.IO.Path.GetExtension(file.FileName)?.TrimStart('.').ToLower() ?? "unknown";

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var user = await _userRepository.Get(username);
            if (user == null) throw new Exception("User not found.");
            if (user.Role != "Admin") throw new UnAuthorizedAccessException("User does not have permission to upload files.");
            var fileModel = new FileModel
            {
                FileName = file.FileName,
                Size = fileSize,
                FileType = fileType,
                FileContent = memoryStream.ToArray(),
                UploadedAt = DateTime.UtcNow,
                UploadedBy = user.UserName
            };

            await _fileRepository.Add(fileModel);
            return fileModel;
        } catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<FileModel> GetFile(int id)
    {
        try
        {
        if (id <= 0)
            throw new ArgumentException("Invalid file ID.");
            return await _fileRepository.Get(id);
        } catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<IEnumerable<object>> GetAll()
    {
        var files = await _fileRepository.GetAll();
        return files.Select(f => new {
            f.Id,
            f.FileName,
            f.Size,
            f.FileType,
            f.UploadedAt,
            f.UploadedBy,
            DownloadUrl = $"http://localhost:5124/api/fileoperation/download/{f.Id}"
        });
    }

}