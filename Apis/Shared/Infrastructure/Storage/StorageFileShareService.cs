using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Shared.Domain.Abstractions.FileStorage;

namespace Shared.Infrastructure.Storage
{
    public class StorageFileShareService : IStorageFileShareService
    {
        public async Task<byte[]> DownloadBytesAsync(
            string connectionString,
            string shareName,
            string dirName,
            string fileName
        )
        {
            ShareClient share = new ShareClient(connectionString, shareName);
            ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
            ShareFileClient file = directory.GetFileClient(fileName);
            Response<ShareFileDownloadInfo> downloadResponse = await file.DownloadAsync();
            ShareFileDownloadInfo downloadInfo = downloadResponse.Value;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await downloadInfo.Content.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task<string> DownloadStringAsync(
            string connectionString,
            string shareName,
            string dirName,
            string fileName
        )
        {
            ShareClient share = new ShareClient(connectionString, shareName);
            ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
            ShareFileClient file = directory.GetFileClient(fileName);
            Stream stream = await file.OpenReadAsync();
            return await new StreamReader(stream).ReadToEndAsync();
        }
    }
}
