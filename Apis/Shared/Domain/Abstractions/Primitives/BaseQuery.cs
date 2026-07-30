using Shared.Domain.Abstractions.Enumerations;

namespace Shared.Domain.Abstractions.Primitives
{
    public class BaseQuery
    {
        public int Offset { get; set; } = 1;
        public int Limit { get; set; } = 4;
        public SortBy SortBy { get; set; } = SortBy.Id;
        public bool SortOrderAscending { get; set; } = false;
    }
}
