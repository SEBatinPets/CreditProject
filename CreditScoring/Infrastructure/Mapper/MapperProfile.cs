using AutoMapper;
using CreditProjectRequestsModels.Models.EvaluateRequest.Request;
using ServiceCreditRequest.Models.Entities;

namespace CreditScoring.Infrastructure.Mapper
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<CreditRequestEvaluateRequest, CreditRequest>();
            CreateMap<CreditContractEvaluateRequest, CreditContract>();
            CreateMap<CreditApplicantEvaluateRequest, CreditApplicant>();
        }
    }
}
