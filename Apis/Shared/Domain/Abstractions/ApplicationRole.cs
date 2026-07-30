using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Shared.Domain.Abstractions
{
    [Table("Roles", Schema = "Users")]
    public partial class ApplicationRole : IdentityRole
    {
        [JsonIgnore]
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
