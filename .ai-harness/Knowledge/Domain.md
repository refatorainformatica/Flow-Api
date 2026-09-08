# Domain — Flow-Api

> Glossário enriquecido em 2026-08-24 a partir de features + código.

## Glossário

| Termo | Significado | Origem |
|-------|-------------|--------|
| Harness | Estrutura `.ai-harness/` deste repositório | `—` |
| Feature | Unidade em `Specification/features/<id>/` | `—` |
| Run | Execução do agente em `Runtime/state/runs/` | `—` |
| Acceptance | Critérios derivados de testes unitários (AC-T*) | `—` |
| financials-banks | API module BanksModule | `Apis/Services/Features/Financials/Banks` |
| financials-cashflows | API module CashFlowsModule | `Apis/Services/Features/Financials/CashFlows` |
| financials-contractstates | API module ContractStatesModule | `Apis/Services/Features/Financials/ContractStates` |
| financials-contracttypes | API module ContractTypesModule | `Apis/Services/Features/Financials/ContractTypes` |
| financials-contracts | API module ContractsModule | `Apis/Services/Features/Financials/Contracts` |
| financials-costcenters | API module CostCentersModule | `Apis/Services/Features/Financials/CostCenters` |
| financials-currencytypes | API module CurrencyTypesModule | `Apis/Services/Features/Financials/CurrencyTypes` |
| financials-expensetypes | API module ExpenseTypesModule | `Apis/Services/Features/Financials/ExpenseTypes` |
| financials-expenses | API module ExpensesModule | `Apis/Services/Features/Financials/Expenses` |
| financials-invoicestates | API module InvoiceStatesModule | `Apis/Services/Features/Financials/InvoiceStates` |
| financials-invoicetypes | API module InvoiceTypesModule | `Apis/Services/Features/Financials/InvoiceTypes` |
| financials-invoices | API module InvoicesModule | `Apis/Services/Features/Financials/Invoices` |
| financials-movementtypes | API module MovementTypesModule | `Apis/Services/Features/Financials/MovementTypes` |
| financials-paymentstates | API module PaymentStatesModule | `Apis/Services/Features/Financials/PaymentStates` |
| financials-revenuetypes | API module RevenueTypesModule | `Apis/Services/Features/Financials/RevenueTypes` |
| financials-revenues | API module RevenuesModule | `Apis/Services/Features/Financials/Revenues` |
| peoples-activitybranchs | API module ActivityBranchsModule | `Apis/Services/Features/Peoples/ActivityBranchs` |
| peoples-careers | API module CareersModule | `Apis/Services/Features/Peoples/Careers` |
| peoples-customers | API module CustomersModule | `Apis/Services/Features/Peoples/Customers` |
| peoples-educationlevels | API module EducationLevelsModule | `Apis/Services/Features/Peoples/EducationLevels` |
| peoples-juridicalnatures | API module JuridicalNaturesModule | `Apis/Services/Features/Peoples/JuridicalNatures` |
| peoples-maritalstates | API module MaritalStatesModule | `Apis/Services/Features/Peoples/MaritalStates` |
| peoples-professionalprofiles | API module ProfessionalProfilesModule | `Apis/Services/Features/Peoples/ProfessionalProfiles` |
| peoples-sellers | API module SellersModule | `Apis/Services/Features/Peoples/Sellers` |
| peoples-skillcategories | API module SkillCategoriesModule | `Apis/Services/Features/Peoples/SkillCategories` |
| peoples-skilllevels | API module SkillLevelsModule | `Apis/Services/Features/Peoples/SkillLevels` |
| peoples-skillstates | API module SkillStatesModule | `Apis/Services/Features/Peoples/SkillStates` |
| peoples-skilltypes | API module SkillTypesModule | `Apis/Services/Features/Peoples/SkillTypes` |
| peoples-skills | API module SkillsModule | `Apis/Services/Features/Peoples/Skills` |
| peoples-sponsors | API module SponsorsModule | `Apis/Services/Features/Peoples/Sponsors` |
| peoples-suppliers | API module SuppliersModule | `Apis/Services/Features/Peoples/Suppliers` |
| peoples-talents | API module TalentsModule | `Apis/Services/Features/Peoples/Talents` |
| settings-documenttypes | API module DocumentTypesModule | `Apis/Services/Features/Settings/DocumentTypes` |
| abstractions-modules | API module IModule | `Apis/Shared/Domain/Abstractions/Modules` |
| ServiceRegistration | class em `Apis/Shared/ServiceRegistration.cs` | `Apis/Shared/ServiceRegistration.cs` |
| Program | class em `Apis/Application/Program.cs` | `Apis/Application/Program.cs` |
| ServiceExtensions | class em `Apis/Services/ServiceExtensions.cs` | `Apis/Services/ServiceExtensions.cs` |
| CountryMappingProfile | class em `Apis/Services/Mappings/Settings/CountryMappingProfile.cs` | `Apis/Services/Mappings/Settings/CountryMappingProfile.cs` |
| DayOffMappingProfile | class em `Apis/Services/Mappings/Settings/DayOffMappingProfile.cs` | `Apis/Services/Mappings/Settings/DayOffMappingProfile.cs` |
| CurrencyTypeMappingProfile | class em `Apis/Services/Mappings/Settings/CurrencyTypeMappingProfile.cs` | `Apis/Services/Mappings/Settings/CurrencyTypeMappingProfile.cs` |
| UserMappingProfile | class em `Apis/Services/Mappings/Users/UserMappingProfile.cs` | `Apis/Services/Mappings/Users/UserMappingProfile.cs` |
| TaskMappingProfile | class em `Apis/Services/Mappings/Projects/TaskMappingProfile.cs` | `Apis/Services/Mappings/Projects/TaskMappingProfile.cs` |
| ScopeTypeMappingProfile | class em `Apis/Services/Mappings/Projects/ScopeTypeMappingProfile.cs` | `Apis/Services/Mappings/Projects/ScopeTypeMappingProfile.cs` |
| StateMappingProfile | class em `Apis/Services/Mappings/Projects/StateMappingProfile.cs` | `Apis/Services/Mappings/Projects/StateMappingProfile.cs` |
| ProjectTaskTypeMappingProfile | class em `Apis/Services/Mappings/Projects/ProjectTaskTypeMappingProfile.cs` | `Apis/Services/Mappings/Projects/ProjectTaskTypeMappingProfile.cs` |
| PriorityTypeMappingProfile | class em `Apis/Services/Mappings/Projects/PriorityTypeMappingProfile.cs` | `Apis/Services/Mappings/Projects/PriorityTypeMappingProfile.cs` |
| AllocationMappingProfile | class em `Apis/Services/Mappings/Projects/AllocationMappingProfile.cs` | `Apis/Services/Mappings/Projects/AllocationMappingProfile.cs` |
| TimesheetMappingProfile | class em `Apis/Services/Mappings/Projects/TimesheetMappingProfile.cs` | `Apis/Services/Mappings/Projects/TimesheetMappingProfile.cs` |
| SprintMappingProfile | class em `Apis/Services/Mappings/Projects/SprintMappingProfile.cs` | `Apis/Services/Mappings/Projects/SprintMappingProfile.cs` |
| ProjectTaskStateMappingProfile | class em `Apis/Services/Mappings/Projects/ProjectTaskStateMappingProfile.cs` | `Apis/Services/Mappings/Projects/ProjectTaskStateMappingProfile.cs` |
| ProjectMappingProfile | class em `Apis/Services/Mappings/Projects/ProjectMappingProfile.cs` | `Apis/Services/Mappings/Projects/ProjectMappingProfile.cs` |
| AllocationStateMappingProfile | class em `Apis/Services/Mappings/Projects/AllocationStateMappingProfile.cs` | `Apis/Services/Mappings/Projects/AllocationStateMappingProfile.cs` |
| TimesheetStateMappingProfile | class em `Apis/Services/Mappings/Projects/TimesheetStateMappingProfile.cs` | `Apis/Services/Mappings/Projects/TimesheetStateMappingProfile.cs` |
| MethodologyTypeMappingProfile | class em `Apis/Services/Mappings/Projects/MethodologyTypeMappingProfile.cs` | `Apis/Services/Mappings/Projects/MethodologyTypeMappingProfile.cs` |
| SalesTaskTypeMappingProfile | class em `Apis/Services/Mappings/Sales/SalesTaskTypeMappingProfile.cs` | `Apis/Services/Mappings/Sales/SalesTaskTypeMappingProfile.cs` |
| OpportunityStateMappingProfile | class em `Apis/Services/Mappings/Sales/OpportunityStateMappingProfile.cs` | `Apis/Services/Mappings/Sales/OpportunityStateMappingProfile.cs` |

## Como manter

1. Novo conceito de domínio → linha neste glossário **na mesma PR**.
2. Mesmo identificador em SPEC, código e testes.
3. ADR se o termo implica trade-off arquitetural.
