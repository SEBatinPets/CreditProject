using AutoMapper;
using CreditProjectRequestsModels.Models.CreateRequest.Request;
using CreditProjectRequestsModels.Models.EvaluateRequest.Request;
using CreditProjectRequestsModels.Models.StatusRequest.Response;
using ServiceCreditRequest.Models.Entities;

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
