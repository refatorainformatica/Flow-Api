namespace Shared.Domain.Abstractions.Enumerations
{
    public enum ErrorType
    {
        None,
        Failure,
        Validation,
        Unauthorized,
        NotFound,
        NoContent,
        PreConditionFailed,
    }
}
