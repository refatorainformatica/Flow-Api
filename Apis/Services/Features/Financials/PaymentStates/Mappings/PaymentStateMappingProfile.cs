using AutoMapper;
using Services.Features.Financials.PaymentStates.Models;
using Services.Features.Financials.PaymentStates.UseCases.Commands;
using Services.Features.Financials.PaymentStates.UseCases.Queries;

namespace Services.Features.Financials.PaymentStates.Mappings
{
    public class PaymentStateMappingProfile : Profile
    {
        public PaymentStateMappingProfile()
        {
            CreateMap<PaymentState, CreatePaymentStateRequest>();
            CreateMap<PaymentState, EditPaymentStateRequest>()
                .ForMember(member => member.RequestId, map => map.MapFrom(x => x.Id));
            CreateMap<PaymentState, RemovePaymentStateRequest>();
            CreateMap<PaymentState, GetByIdPaymentStateRequest>();
            CreateMap<PaymentState, GetPaymentStateRequest>();
            CreateMap<PaymentState, GetBySearchPaymentStateRequest>();

            CreateMap<PaymentState, PaymentStateResponse>();
        }
    }
}
