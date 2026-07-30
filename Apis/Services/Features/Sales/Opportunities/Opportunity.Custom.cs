using System.ComponentModel.DataAnnotations.Schema;
using Shared.Domain.Abstractions;

namespace Services.Features.Sales.Opportunities
{
    public partial class Opportunity
    {
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
