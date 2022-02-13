using CreditProjectRequestsModels.Models.ScoringResultRequest.Request;
using System.Threading.Tasks;

namespace CreditScoring.Data.SendResult.Interfaces
{
    public interface ISendScoringResult
    {
        public Task SendResultAsync(ScoringResultRequest result, string creditServiceUri);
    }
}
