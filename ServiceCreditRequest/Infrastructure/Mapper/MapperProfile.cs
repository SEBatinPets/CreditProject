using AutoMapper;
using ServiceCreditRequest.Models.Entities;
using ServiceCreditRequest.Models.Incoming.Request;
using ServiceCreditRequest.Models.Outcoming.EvaluateRequest.Request;
using ServiceCreditRequest.Models.Outcoming.Response;

namespace ServiceCreditRequest.Infrastructure.Mapper
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<CreditRequestCreateRequest, CreditRequest>();
            CreateMap<CreditApplicantCreateRequest, CreditApplicant>();
            CreateMap<CreditContractCreateRequest, CreditContract>();

            CreateMap<CreditRequest, CreditRequestStatusResponse>();
            CreateMap<CreditApplicant, CreditApplicantStatusResponse>();
            CreateMap<CreditContract, CreditContractStatusResponse>();

            CreateMap<CreditRequest, CreditRequestEvaluateRequest>();
            CreateMap<CreditApplicant, CreditApplicantEvaluateRequest>();
            CreateMap<CreditContract, CreditContractEvaluateRequest>();
        }
    }
}
