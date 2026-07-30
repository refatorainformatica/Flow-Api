namespace Shared.Domain.Abstractions.Bus
{
    public interface IMemoryBus : ICommandHandler, IEventHandler { }
}
