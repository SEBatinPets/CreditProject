using AutoMapper;
using ServiceCreditRequest.Models.Entities;
using ServiceCreditRequest.Models.Incoming.Request;

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
