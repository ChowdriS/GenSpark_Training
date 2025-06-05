using System;

namespace AppointmentApi.Interface;

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file);
    byte[] GetFile(string fileName);
}
