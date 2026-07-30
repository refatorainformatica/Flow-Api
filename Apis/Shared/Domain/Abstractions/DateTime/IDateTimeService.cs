namespace Shared.Domain.Abstractions.DateTime
{
    public interface IDateTimeService
    {
        System.DateTime UtcNow { get; }
    }
}
