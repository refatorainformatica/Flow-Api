namespace Shared.Domain.Abstractions.Primitives
{
    public class BaseResponse
    {
        public int Id { get; set; }
        public System.DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime? EditedAt { get; set; }
        public string EditedBy { get; set; }
        public System.DateTime? DeletedAt { get; set; }
        public string Picture { get; set; }
    }
}
