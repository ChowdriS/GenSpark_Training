using System;
using Azure.Storage.Blobs;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace blobstorage.Services;

public class BlobStorageService
{
    private BlobContainerClient _containerClient;
    private readonly IConfiguration _configuration;

    public BlobStorageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private async Task UpdateContainerClient()
    {
        var keyVaultUrl = _configuration["AzureBlob:KeyVaultUrl"];
        var containerName = "images";
        var secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
        KeyVaultSecret secret = await secretClient.GetSecretAsync("connectionstring"); 
        string connectionString = secret.Value;

        _containerClient = new BlobContainerClient(connectionString, containerName);
    }

    public async Task UploadFile(Stream fileStream, string fileName)
    {
        await UpdateContainerClient();
        var blobClient = _containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true);
    }

    public async Task<Stream> DownloadFile(string fileName)
    {
        await UpdateContainerClient();
        var blobClient = _containerClient.GetBlobClient(fileName);
        if (await blobClient.ExistsAsync())
        {
            var downloadInfo = await blobClient.DownloadStreamingAsync();
            return downloadInfo.Value.Content;
        }
        return null;
    }
}
