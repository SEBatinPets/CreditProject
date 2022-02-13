using ServiceCreditRequest.Models.Outcoming.EvaluateRequest.Request;
using System.Threading.Tasks;

namespace ServiceCreditRequest.Client
{
    public interface ICreditRequestClient
    {
        Task SendEvaluateRequest(CreditRequestEvaluateRequest creditRequest);
    }
}
