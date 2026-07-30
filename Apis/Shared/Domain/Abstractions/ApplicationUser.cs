using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Shared.Domain.Abstractions
{
    [Table("Users", Schema = "Users")]
    public partial class ApplicationUser : IdentityUser
    {
        [JsonIgnore, IgnoreDataMember]
        public override string PasswordHash { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }

        [JsonIgnore, IgnoreDataMember, NotMapped]
        public string Name
        {
            get { return UserName; }
            set { UserName = value; }
        }

        public ICollection<ApplicationRole> Roles { get; set; }

        //[JsonIgnore]
        //[InverseProperty(nameof(TestResult.User))]
        //public virtual ICollection<TestResult> TestResults { get; set; }

        //[JsonIgnore]
        //[InverseProperty(nameof(UserBadge.User))]
        //public virtual ICollection<UserBadge> UserBadges { get; set; }

        //[JsonIgnore]
        //[InverseProperty(nameof(UserResponse.User))]
        //public virtual ICollection<UserResponse> UserResponses { get; set; }
    }
}
