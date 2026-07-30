using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Shared.Domain.Abstractions.FileStorage;

namespace Shared.Infrastructure.Storage
{
    public class StorageBlobService : IStorageBlobService
    {
        public async Task<string> UploadAsync(
            string connectionString,
            string container,
            string base64FileData,
            string fileExtension
        )
        {
            base64FileData = base64FileData.Replace("data:image/[a-z]+;base64,", "");

            var fileName = $"{Guid.NewGuid()}.{fileExtension}";
            var imageBytes = Convert.FromBase64String(base64FileData);
            var blobClient = new BlobClient(connectionString, container, fileName);

            await using var stream = new MemoryStream(imageBytes);
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task DeleteAsync(string connectionString, string container, string fileName)
        {
            var blobClient = new BlobClient(
                connectionString,
                container,
                Path.GetFileName(fileName)
            );
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
