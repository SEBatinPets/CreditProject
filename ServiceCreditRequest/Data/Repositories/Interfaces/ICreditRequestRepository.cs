using ServiceCreditRequest.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceCreditRequest.Data.Repositories.Interfaces
{
    public interface ICreditRequestRepository: IRepository<CreditRequest>
    {
        Task UpdateScoringAsync(bool scoringResult, DateTime scoringDate, int id);
        Task<IEnumerable<int>> GetIdByScoringStatusAsync(bool? scoringResult);
        Task<CreditRequest> GetByApplicationNumAsync(string applicantNum);
    }
}
