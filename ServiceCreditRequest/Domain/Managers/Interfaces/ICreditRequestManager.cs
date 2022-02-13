using CreditProjectRequestsModels.Models.CreateRequest.Request;
using CreditProjectRequestsModels.Models.EvaluateRequest.Request;
using CreditProjectRequestsModels.Models.ScoringResultRequest.Request;
using CreditProjectRequestsModels.Models.StatusRequest.Response;
using ServiceCreditRequest.Models.Entities;
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
