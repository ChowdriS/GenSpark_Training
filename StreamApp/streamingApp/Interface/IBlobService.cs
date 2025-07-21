using System;

namespace streamingApp.Interface;

public interface IBlobService
{
    public Task<string> UploadFile(Stream fileStream, string fileName);
}
