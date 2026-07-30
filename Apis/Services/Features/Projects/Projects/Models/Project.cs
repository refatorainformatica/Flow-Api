using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Services.Features.Peoples.Customers.Models;
using Services.Features.Projects.Allocations;
using Services.Features.Projects.MethodologyTypes;
using Services.Features.Projects.Projects.Models.Events;
using Services.Features.Projects.ProjectStates;
using Services.Features.Projects.ProjectTasks;
using Services.Features.Projects.ScopeTypes;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Projects.Projects.Models
{
    [Table("Projects", Schema = "Projects")]
    public partial class Project : BaseEntity
    {
        public Project()
        {
            Allocations = [];
            ProjectTasks = [];
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Surname { get; set; }

        [Required]
        public string Description { get; set; }

        public int CustomerId { get; set; }

        public int PriorityTypeId { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? EstimatedTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public int? ProjectStateId { get; set; }

        public int? MethodologyTypeId { get; set; }

        [Column(TypeName = "money")]
        public decimal? InvestmentValue { get; set; }

        public int? ScopeTypeId { get; set; }

        public string Picture { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customer.Projects))]
        public virtual Customer Customer { get; set; }

        [ForeignKey(nameof(MethodologyTypeId))]
        [InverseProperty(nameof(MethodologyType.Projects))]
        public virtual MethodologyType MethodologyType { get; set; }

        [ForeignKey(nameof(ProjectStateId))]
        [InverseProperty(nameof(ProjectState.Projects))]
        public virtual ProjectState ProjectState { get; set; }

        [ForeignKey(nameof(ScopeTypeId))]
        [InverseProperty(nameof(ScopeType.Projects))]
        public virtual ScopeType ScopeType { get; set; }

        [InverseProperty(nameof(Allocation.Project))]
        public virtual ICollection<Allocation> Allocations { get; set; }

        [InverseProperty(nameof(ProjectTask.Project))]
        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }

        public override void OnCreatedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ProjectCreatedEvent(Id));
        }

        public override void OnEditedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ProjectEditedEvent(Id));
        }

        public override void OnRemovedEvent()
        {
            if (Id is 0)
                throw new BusinessRuleException($"{nameof(Id)} is required");

            AddEvent(new ProjectRemovedEvent(Id));
        }
    }
}
