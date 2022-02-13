using AutoMapper;
using CreditScoring.Data.Repository.Interfaces;
using CreditScoring.Domain.Managers.Interfaces;
using Microsoft.Extensions.Logging;
using ServiceCreditRequest.Models.Entities;
using ServiceCreditRequest.Models.Incoming.Request;
using System;
using System.Threading.Tasks;

namespace CreditScoring.Domain.Managers.Implementation
{
    /// <summary>
    /// middleware для обработки моделей заявки на кредит
    /// </summary>
    public class CreditScoringManager : ICreditScoringManager
    {
        private readonly IScoringRepository scoringRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CreditScoringManager> logger;
        public CreditScoringManager(IScoringRepository scoringRepository, IMapper mapper, ILogger<CreditScoringManager> logger)
        {
            this.scoringRepository = scoringRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public Task ScoringAsync(CreditRequestEvaluateRequest item)
        {
            logger.LogInformation($"Received credit request {item.ApplicationNum}");

            var scoring = mapper.Map<CreditRequest>(item);

            Task.Run(() => scoringRepository.CreateAsync(scoring));

            return Task.CompletedTask;
        }
    }
}
