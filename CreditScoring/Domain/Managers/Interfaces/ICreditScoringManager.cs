using CreditProjectRequestsModels.Models.EvaluateRequest.Request;
using System.Threading.Tasks;

namespace CreditScoring.Domain.Managers.Interfaces
{
    public interface ICreditScoringManager
    {
        public Task ScoringAsync(CreditRequestEvaluateRequest item);
    }
}
