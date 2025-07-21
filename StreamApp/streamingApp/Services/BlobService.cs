using System;
using Azure.Storage.Blobs;
using streamingApp.Interface;

namespace streamingApp.Services;

public class BlobService : IBlobService
{
    private readonly IConfiguration _configuration;
    private BlobContainerClient _container;

    public BlobService(IConfiguration configuration)
    {
        _configuration = configuration;
        var connString = _configuration["Azure:BlobConnectionString"];
        var containerName = _configuration["Azure:BlobContainer"];
        _container = new BlobContainerClient(connString, containerName);
    }

    public async Task<string> UploadFile(Stream fileStream, string fileName)
    {
        var blobClient = _container.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true);
        return blobClient.Uri.ToString();
    }
}
