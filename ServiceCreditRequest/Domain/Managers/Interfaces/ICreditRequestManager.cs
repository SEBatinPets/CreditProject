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
        public Task<int> CreateAsync(CreditRequestCreateRequest item);
        public Task<CreditRequestStatusResponse> GetScoringByIdAsync (int id);
        public Task UpdateScoringAsync(ScoringResultRequest item);
        public Task<IEnumerable<CreditRequest>> GetByScoringStatusAsync(bool? scoringStatus);
        public Task<IEnumerable<CreditRequestEvaluateRequest>> GetForEvaluateRequestAsync();
        public Task<CreditRequest> GetByIdAsync(int id);
    }
}
