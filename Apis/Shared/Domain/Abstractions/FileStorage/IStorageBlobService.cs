using System.Threading.Tasks;

namespace Shared.Domain.Abstractions.FileStorage
{
    public interface IStorageBlobService
    {
        Task<string> UploadAsync(
            string connectionString,
            string container,
            string base64FileData,
            string fileExtension
        );

        Task DeleteAsync(string connectionString, string container, string fileName);
    }
}
