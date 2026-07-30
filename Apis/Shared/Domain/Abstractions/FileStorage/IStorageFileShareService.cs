using System.Threading.Tasks;

namespace Shared.Domain.Abstractions.FileStorage
{
    public interface IStorageFileShareService
    {
        Task<string> DownloadStringAsync(
            string connectionString,
            string shareName,
            string dirName,
            string fileName
        );

        Task<byte[]> DownloadBytesAsync(
            string connectionString,
            string shareName,
            string dirName,
            string fileName
        );
    }
}
