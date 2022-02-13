using ServiceCreditRequest.Models.Entities;
using ServiceCreditRequest.Models.Incoming.Request;
using ServiceCreditRequest.Models.Incoming.Response;
using ServiceCreditRequest.Models.Outcoming.EvaluateRequest.Request;
using ServiceCreditRequest.Models.Outcoming.Response;
using ServiceCreditRequest.Models.ScoringResult.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceCreditRequest.Domain.Managers.Interfaces
{
    public interface ICreditRequestManager
    {
        public Task<int> Create(CreditRequestCreateRequest item);
        public Task<CreditRequestStatusResponse> GetScoringById (int id);
        public Task UpdateScoring(ScoringResultRequest item);
        public Task<IEnumerable<CreditRequest>> GetByScoringStatus(bool? scoringStatus);
        public Task<IEnumerable<CreditRequestEvaluateRequest>> GetForEvaluateRequest();
        public Task<CreditRequest> GetById(int id);
    }
}
