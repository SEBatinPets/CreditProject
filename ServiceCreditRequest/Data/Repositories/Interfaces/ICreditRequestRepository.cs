using ServiceCreditRequest.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceCreditRequest.Data.Repositories.Interfaces
{
    public interface ICreditRequestRepository: IRepository<CreditRequest>
    {
        Task UpdateScoring(bool scoringResult, DateTime scoringDate, int id);
        Task<IEnumerable<int>> GetIdByScoringStatus(bool? scoringResult);
    }
}
