using CreditScoring.Data.SendResult.Interfaces;
using CreditScoring.Models.ScoringResultRequest.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CreditScoring.Data.SendResult.Implementation
{
    public class SendScoringResult: ISendScoringResult
    {
        private readonly HttpClient httpClient;
        private readonly string apiUri;
        private readonly ILogger<SendScoringResult> logger;
        public SendScoringResult(HttpClient httpClient, IConfiguration configuration, ILogger<SendScoringResult> logger)
        {
            this.httpClient = httpClient;
            apiUri = configuration["CreditEvaluate:apiUri"];
            this.logger = logger;
        }
        /// <summary>
        /// отправка результатов скоринга
        /// </summary>
        /// <param name="result">результат скоринга</param>
        /// <param name="creditServiceUri">получатель скоринга</param>
        /// <returns></returns>
        public async Task SendResultAsync(ScoringResultRequest result, string creditServiceUri)
        {
            logger.LogInformation($"Sending evaluate {result.ApplicationNum} to {creditServiceUri}");

            var httpRequest = new HttpRequestMessage(HttpMethod.Put, $"{creditServiceUri}/{apiUri}");
            var json = JsonSerializer.Serialize(result);
            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                await httpClient.SendAsync(httpRequest);
            }
            catch(Exception ex)
            {
                logger.LogWarning(ex.ToString());
            }
        }
    }
}
