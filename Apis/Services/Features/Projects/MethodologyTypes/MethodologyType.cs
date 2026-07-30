using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Projects.MethodologyTypes.Events;
using Services.Features.Projects.Projects.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Projects.MethodologyTypes
{
    [Table("MethodologyTypes", Schema = "Projects")]
    public partial class MethodologyType : BaseEntity
    {
        public MethodologyType()
        {
            Projects = [];
        }

        [StringLength(256)]
        public string Description { get; set; }

        public string Picture { get; set; }

        [InverseProperty(nameof(Project.MethodologyType))]
        public virtual ICollection<Project> Projects { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new MethodologyTypeCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new MethodologyTypeEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new MethodologyTypeRemovedEvent(Id));
        }
    }
}
