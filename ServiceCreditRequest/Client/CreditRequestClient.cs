using CreditProjectRequestsModels.Models.EvaluateRequest.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceCreditRequest.Client
{
    /// <summary>
    /// http клиент для отправки заявок на скоринг
    /// </summary>
    public class CreditRequestClient : ICreditRequestClient
    {
        private readonly HttpClient httpClient;
        private readonly string evaluateUri;
        private readonly string scoringServiceUri;
        private readonly ILogger<CreditRequestClient> logger;
        public CreditRequestClient(
            HttpClient httpClient, 
            IConfiguration configuration,
            ILogger<CreditRequestClient> logger)
        {
            this.httpClient = httpClient;
            this.evaluateUri = configuration["ScoringService:EvaluateUri"];
            this.scoringServiceUri = configuration["ScoringService:ScoringServiceUri"];
            this.logger = logger;
        }
        /// <summary>
        /// отправка заявки на скоринг
        /// </summary>
        /// <param name="creditRequest">заявка на кредит</param>
        /// <returns></returns>
        public async Task SendEvaluateRequestAsync(CreditRequestEvaluateRequest creditRequest)
        {
            logger.LogInformation($"Send scoring request " +
                $"applicant {creditRequest.ApplicationNum} id {creditRequest.Id}");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{scoringServiceUri}/{evaluateUri}");
            var json = JsonSerializer.Serialize(creditRequest);
            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                await httpClient.SendAsync(httpRequest);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex.ToString());
            }
        }
    }
}
