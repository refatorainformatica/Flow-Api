//using Microsoft.EntityFrameworkCore;

//namespace Shared.Infrastructure.Persistences
//{
//    public partial class ApplicationDbContext : DbContext
//    {
//        public ApplicationDbContext() { }

//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//            : base(options) { }

//        public virtual DbSet<ActivityBranch> ActivityBranchs { get; set; }
//        public virtual DbSet<Allocation> Allocations { get; set; }
//        public virtual DbSet<AllocationState> AllocationStates { get; set; }
//        public virtual DbSet<Badge> Badges { get; set; }
//        public virtual DbSet<Bank> Banks { get; set; }
//        public virtual DbSet<Career> Careers { get; set; }
//        public virtual DbSet<CashFlow> CashFlows { get; set; }
//        public virtual DbSet<City> Cities { get; set; }
//        public virtual DbSet<Contact> Contacts { get; set; }
//        public virtual DbSet<Contract> Contracts { get; set; }
//        public virtual DbSet<ContractDocument> ContractDocuments { get; set; }
//        public virtual DbSet<ContractState> ContractStates { get; set; }
//        public virtual DbSet<ContractSubscription> ContractSubscriptions { get; set; }
//        public virtual DbSet<ContractType> ContractTypes { get; set; }
//        public virtual DbSet<CostCenter> CostCenters { get; set; }
//        public virtual DbSet<Country> Countries { get; set; }
//        public virtual DbSet<Course> Courses { get; set; }
//        public virtual DbSet<CourseModule> CourseModules { get; set; }
//        public virtual DbSet<CoursesBadge> CoursesBadges { get; set; }
//        public virtual DbSet<CurrencyType> CurrencyTypes { get; set; }
//        public virtual DbSet<Customer> Customers { get; set; }
//        public virtual DbSet<CustomerDocument> CustomerDocuments { get; set; }
//        public virtual DbSet<DayOff> DayOffs { get; set; }
//        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
//        public virtual DbSet<EducationLevel> EducationLevels { get; set; }
//        public virtual DbSet<Estate> Estates { get; set; }
//        public virtual DbSet<Expense> Expenses { get; set; }
//        public virtual DbSet<ExpenseType> ExpenseTypes { get; set; }
//        public virtual DbSet<Invoice> Invoices { get; set; }
//        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }
//        public virtual DbSet<InvoiceState> InvoiceStates { get; set; }
//        public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }
//        public virtual DbSet<JuridicalNature> JuridicalNatures { get; set; }
//        public virtual DbSet<KnowledgeTest> KnowledgeTests { get; set; }
//        public virtual DbSet<MaritalState> MaritalStates { get; set; }
//        public virtual DbSet<MethodologyType> MethodologyTypes { get; set; }
//        public virtual DbSet<MovementType> MovementTypes { get; set; }
//        public virtual DbSet<Opportunity> Opportunities { get; set; }
//        public virtual DbSet<OpportunityState> OpportunityStates { get; set; }
//        public virtual DbSet<PaymentState> PaymentStatees { get; set; }
//        public virtual DbSet<PriorityType> PriorityTypes { get; set; }
//        public virtual DbSet<ProfessionalProfile> ProfessionalProfiles { get; set; }
//        public virtual DbSet<Project> Projects { get; set; }
//        public virtual DbSet<ProjectState> ProjectStates { get; set; }
//        public virtual DbSet<ProjectTask> ProjectTasks { get; set; }
//        public virtual DbSet<ProjectTaskType> ProjectTaskTypes { get; set; }
//        public virtual DbSet<Question> Questions { get; set; }
//        public virtual DbSet<QuestionResponse> QuestionResponses { get; set; }
//        public virtual DbSet<Revenue> Revenues { get; set; }
//        public virtual DbSet<RevenueType> RevenueTypes { get; set; }
//        public virtual DbSet<RoadType> RoadTypes { get; set; }
//        public virtual DbSet<SaleTask> SaleTasks { get; set; }
//        public virtual DbSet<SaleTaskState> SaleTaskStates { get; set; }
//        public virtual DbSet<SaleTaskType> SaleTaskTypes { get; set; }
//        public virtual DbSet<ScopeType> ScopeTypes { get; set; }
//        public virtual DbSet<Seller> Sellers { get; set; }
//        public virtual DbSet<SellerDocument> SellerDocuments { get; set; }
//        public virtual DbSet<Setting> Settings { get; set; }
//        public virtual DbSet<Skill> Skills { get; set; }
//        public virtual DbSet<SkillCategory> SkillCategories { get; set; }
//        public virtual DbSet<SkillLevel> SkillLevels { get; set; }
//        public virtual DbSet<SkillState> SkillStates { get; set; }
//        public virtual DbSet<SkillType> SkillTypes { get; set; }
//        public virtual DbSet<Sponsor> Sponsors { get; set; }
//        public virtual DbSet<SponsorDocument> SponsorDocuments { get; set; }
//        public virtual DbSet<Sprint> Sprints { get; set; }
//        public virtual DbSet<SprintTask> SprintTasks { get; set; }
//        public virtual DbSet<Supplier> Suppliers { get; set; }
//        public virtual DbSet<SupplierDocument> SupplierDocuments { get; set; }
//        public virtual DbSet<Talent> Talents { get; set; }
//        public virtual DbSet<TalentDocument> TalentDocuments { get; set; }
//        public virtual DbSet<TestResult> TestResults { get; set; }
//        public virtual DbSet<Timesheet> Timesheets { get; set; }
//        public virtual DbSet<TimesheetItem> TimesheetItems { get; set; }
//        public virtual DbSet<TimesheetState> TimesheetStates { get; set; }
//        public virtual DbSet<UserBadge> UserBadges { get; set; }
//        public virtual DbSet<UserResponse> UserResponses { get; set; }
//    }
//}
