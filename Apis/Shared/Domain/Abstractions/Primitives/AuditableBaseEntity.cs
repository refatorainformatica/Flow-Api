using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Domain.Abstractions.Primitives
{
    public abstract class AuditableBaseEntity
    {
        [Column(TypeName = "datetime")]
        public System.DateTime? CreatedAt { get; set; } = System.DateTime.Now;

        [StringLength(256)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public System.DateTime? EditedAt { get; set; } = System.DateTime.Now;

        [StringLength(256)]
        public string EditedBy { get; set; }

        [Column(TypeName = "datetime")]
        public System.DateTime? DeletedAt { get; set; } = null;
    }
}
