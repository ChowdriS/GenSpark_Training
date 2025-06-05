using System;
using AppointmentApi.Interface;

namespace AppointmentApi.Service;

public class FileService : IFileService
{
    private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
    
    public FileService()
    {
        if (!Directory.Exists(_storagePath))
            Directory.CreateDirectory(_storagePath);
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                throw new IOException("Invalid file");

            var filePath = Path.Combine(_storagePath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return file.FileName;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public byte[] GetFile(string fileName)
    {
        try
        {
            var filePath = Path.Combine(_storagePath, fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            return File.ReadAllBytes(filePath);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}