using System.Threading.Tasks;

namespace Shared.Domain.Abstractions.Chat
{
    public interface IChatService
    {
        public Task<string> SendMessageAsync(string message);
    }
}
