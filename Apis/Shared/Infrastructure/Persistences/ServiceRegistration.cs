//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Shared.Infrastructure.Persistences.UoW;

//namespace Shared.Infrastructure.Persistences
//{
//    public static class ServiceRegistration
//    {
//        public static void AddPersistenceInfrastructure(
//            this IServiceCollection services,
//            IConfiguration configuration
//        )
//        {
//            var sqlConnectionString = configuration.GetConnectionString("FlowDatabase");

//            if (string.IsNullOrEmpty(sqlConnectionString))
//            {
//                sqlConnectionString = Environment.GetEnvironmentVariable("Database")!;
//            }

//            services.AddDbContext<ApplicationDbContext>(options =>
//                options.UseSqlServer(
//                    sqlConnectionString,
//                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
//                )
//            );
//            services.AddScoped<IUnitOfWork, UnitOfWork>();

//            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

//            #region Financial Repositories
//            services.AddScoped<IBankRepository, BankRepository>();
//            services.AddScoped<IPaymentStateRepository, PaymentStateRepository>();
//            services.AddScoped<ICashFlowRepository, CashFlowRepository>();
//            services.AddScoped<IContractRepository, ContractRepository>();
//            services.AddScoped<IContractStateRepository, ContractStateRepository>();
//            services.AddScoped<IContractTypeRepository, ContractTypeRepository>();
//            services.AddScoped<ICostCenterRepository, CostCenterRepository>();
//            services.AddScoped<IExpenseRepository, ExpenseRepository>();
//            services.AddScoped<IExpenseTypeRepository, ExpenseTypeRepository>();
//            services.AddScoped<IMovementTypeRepository, MovementTypeRepository>();
//            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
//            services.AddScoped<IInvoiceStateRepository, InvoiceStateRepository>();
//            services.AddScoped<IInvoiceTypeRepository, InvoiceTypeRepository>();
//            services.AddScoped<IRevenueRepository, RevenueRepository>();
//            services.AddScoped<IRevenueTypeRepository, RevenueTypeRepository>();
//            #endregion

//            #region Learning Repositories
//            services.AddScoped<IBadgeRepository, BadgeRepository>();
//            services.AddScoped<ICourseRepository, CourseRepository>();
//            services.AddScoped<IKnowledgeTestRepository, KnowledgeTestRepository>();
//            #endregion

//            #region People Repositories
//            services.AddScoped<IActivityBranchRepository, ActivityBranchRepository>();
//            services.AddScoped<ICareerRepository, CareerRepository>();
//            services.AddScoped<ICustomerRepository, CustomerRepository>();
//            services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
//            services.AddScoped<IEducationLevelRepository, EducationLevelRepository>();
//            services.AddScoped<IJuridicalNatureRepository, JuridicalNatureRepository>();
//            services.AddScoped<IMaritalStateRepository, MaritalStateRepository>();
//            services.AddScoped<IProfessionalProfileRepository, ProfessionalProfileRepository>();
//            services.AddScoped<ISellerRepository, SellerRepository>();
//            services.AddScoped<ISponsorRepository, SponsorRepository>();
//            services.AddScoped<ISupplierRepository, SupplierRepository>();
//            services.AddScoped<ITalentRepository, TalentRepository>();
//            services.AddScoped<ISkillCategoryRepository, SkillCategoryRepository>();
//            services.AddScoped<ISkillLevelRepository, SkillLevelRepository>();
//            services.AddScoped<ISkillRepository, SkillRepository>();
//            services.AddScoped<ISkillTypeRepository, SkillTypeRepository>();
//            services.AddScoped<ISkillStateRepository, SkillStateRepository>();
//            #endregion
//        }
//    }
//}
