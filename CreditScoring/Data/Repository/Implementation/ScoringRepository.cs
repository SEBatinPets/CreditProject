using CreditScoring.Data.Repository.Interfaces;
using CreditScoring.Data.SendResult.Interfaces;
using CreditScoring.Models.ScoringResultRequest.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceCreditRequest.Models.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CreditScoring.Data.Repository.Implementation
{
    public class ScoringRepository : IScoringRepository
    {
        private readonly ISendScoringResult sendScoring;
        private readonly ILogger<ScoringRepository> logger;
        private readonly int waitTime;
        private const string agentString = "http://localhost:5010";
        /// <summary>
        /// Репозитория обработки решений о выдаче кредитов
        /// </summary>
        /// <param name="sendScoring">http клиент для отправки решений о выдаче кредита</param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public ScoringRepository(ISendScoringResult sendScoring, IConfiguration configuration, ILogger<ScoringRepository> logger)
        {
            this.sendScoring = sendScoring;
            //получение задержки отправки результата
            bool tryParse = int.TryParse(configuration["CreditEvaluate:WaitingTimeMs"], out waitTime);
            this.logger = logger;
        }
        /// <summary>
        /// создание и обработка заявки на кредит
        /// </summary>
        /// <param name="item">заявка на кредит</param>
        /// <returns></returns>
        public Task CreateAsync(CreditRequest item)
        {
            logger.LogInformation($"Create credit request {item.ApplicationNum}");

            var result = new ScoringResultRequest()
            {
                Id = item.Id,
                ScoringDate = DateTime.Now,
                ScoringStatus = ScoringResult()
            };

            logger.LogInformation($"Credit request {item.ApplicationNum} is {result.ScoringStatus}");

            //асинхронная отправка результата
            Task.Run(() => SendAsync(result));

            return Task.CompletedTask;
        }

        private async Task SendAsync(ScoringResultRequest result)
        {
            await Task.Delay(waitTime);
            await sendScoring.SendResultAsync(result, agentString);
        }

        /// <summary>
        /// случайный результат одобрения
        /// </summary>
        /// <returns></returns>
        private bool ScoringResult()
        {
            Random random = new Random();
            var randNum = random.Next(0, 2);
            switch (randNum)
            {
                case 0:
                    return false;
                case 1:
                    return true;
                default:
                    return false;
            }
        }
    }
}
