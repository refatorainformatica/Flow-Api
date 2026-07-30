using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.Customers.Models.Events;
using Services.Features.Projects.Projects.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Customers.Models
{
    [Table("Customers", Schema = "Peoples")]
    public partial class Customer : BaseEntity
    {
        public Customer()
        {
            Documents = [];
            Projects = [];
        }

        public Customer(
            int id,
            string name,
            string addressLine1,
            string addressLine2,
            string email,
            string phoneNumber,
            string picture,
            DateTime createdAt,
            string createdBy,
            DateTime editedAt,
            string editedBy
        )
        {
            Id = id;
            Name = name;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            Email = email;
            PhoneNumber = phoneNumber;
            Picture = picture;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            EditedAt = editedAt;
            EditedBy = editedBy;
            Documents = [];
            Projects = [];
        }

        public Customer(int id, DateTime editedAt, string editedBy, DateTime deletedAt)
        {
            Id = id;
            EditedAt = editedAt;
            EditedBy = editedBy;
            DeletedAt = deletedAt;
            Documents = [];
            Projects = [];
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(30)]
        public string PhoneNumber { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(CustomerDocument.Customer))]
        public virtual ICollection<CustomerDocument> Documents { get; set; }

        [InverseProperty(nameof(Project.Customer))]
        public virtual ICollection<Project> Projects { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CustomerCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CustomerEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new CustomerRemovedEvent(Id));
        }
    }
}
