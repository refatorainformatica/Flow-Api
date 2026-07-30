namespace Shared.Domain.Abstractions.Primitives
{
    public class Response<T> : PaginationBase
    {
        public Response(T data, int offset = 1, int limit = 1, int pageCount = 1, int rowCount = 1)
        {
            Data = data;
            Offset = offset;
            Limit = limit;
            PageCount = pageCount;
            RowCount = rowCount;
        }

        public T Data { get; set; }
    }
}
