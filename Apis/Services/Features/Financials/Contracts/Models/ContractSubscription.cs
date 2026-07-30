using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Features.Financials.Contracts.Models
{
    [Table("ContractSubscriptions", Schema = "Financials")]
    public partial class ContractSubscription
    {
        [Key]
        public int Id { get; set; }

        public int ContractId { get; set; }

        [Required]
        [StringLength(256)]
        public string SubscriptionCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime SubscriptionDate { get; set; }

        [Required]
        [StringLength(256)]
        public string SubscriptionUser { get; set; }

        public string Picture { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [StringLength(256)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? EditedAt { get; set; }

        [StringLength(256)]
        public string EditedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DeletedAt { get; set; }

        [ForeignKey(nameof(ContractId))]
        [InverseProperty(nameof(Contract.ContractSubscriptions))]
        public virtual Contract Contract { get; set; }
    }
}
