using MediatR;

namespace Shared.Domain.Abstractions.Bus
{
    public interface ICommandHandler
    {
        void SendCommand<T>(T command)
            where T : IRequest;
    }
}
