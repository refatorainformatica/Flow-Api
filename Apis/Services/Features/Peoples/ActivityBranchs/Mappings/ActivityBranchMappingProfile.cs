using AutoMapper;
using Services.Features.Peoples.ActivityBranchs.Models;
using Services.Features.Peoples.ActivityBranchs.UseCases.Commands;
using Services.Features.Peoples.ActivityBranchs.UseCases.Queries;

namespace Services.Features.Peoples.ActivityBranchs.Mappings
{
    public class ActivityBranchMappingProfile : Profile
    {
        public ActivityBranchMappingProfile()
        {
            CreateMap<ActivityBranch, CreateActivityBranchRequest>();
            CreateMap<ActivityBranch, EditActivityBranchRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<ActivityBranch, RemoveActivityBranchRequest>();
            CreateMap<ActivityBranch, GetByIdActivityBranchRequest>();
            CreateMap<ActivityBranch, GetActivityBranchRequest>();
            CreateMap<ActivityBranch, GetBySearchActivityBranchRequest>();

            CreateMap<ActivityBranch, ActivityBranchResponse>();
        }
    }
}
