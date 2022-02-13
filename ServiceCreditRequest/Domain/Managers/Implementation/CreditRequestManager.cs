using AutoMapper;
using CreditProjectRequestsModels.Models.CreateRequest.Request;
using CreditProjectRequestsModels.Models.EvaluateRequest.Request;
using CreditProjectRequestsModels.Models.ScoringResultRequest.Request;
using CreditProjectRequestsModels.Models.StatusRequest.Response;
using Microsoft.Extensions.Logging;
using ServiceCreditRequest.Data.Repositories.Interfaces;
using ServiceCreditRequest.Domain.Managers.Interfaces;
using ServiceCreditRequest.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceCreditRequest.Domain.Managers.Implementation
{

    //TODO рефакторинг: оптимизация репозиториев и менеджера
    public class CreditRequestManager : ICreditRequestManager
    {
        private readonly ICreditRequestRepository requestRepository;
        private readonly ICreditContractRepository contractRepository;
        private readonly ICreditApplicantRepository applicantRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CreditRequestManager> logger;
        public CreditRequestManager(
            ICreditRequestRepository repository, 
            IMapper mapper,
            ICreditContractRepository contractRepository,
            ICreditApplicantRepository applicantRepository,
            ILogger<CreditRequestManager> logger)
        {
            this.requestRepository = repository;
            this.mapper = mapper;
            this.contractRepository = contractRepository;
            this.applicantRepository = applicantRepository;
            this.logger = logger;
        }
        public async Task<int> Create(CreditRequestCreateRequest item)
        {
            logger.LogInformation($"Recieved credit create request {item.ApplicationNum}");

            var creditRequest = mapper.Map<CreditRequest>(item);

            creditRequest.Applicant.Id = await applicantRepository.Create(creditRequest.Applicant);
            creditRequest.RequestedCredit.Id = await contractRepository.Create(creditRequest.RequestedCredit);

            var id = await requestRepository.Create(creditRequest);

            return id;
        }

        public async Task<CreditRequest> GetById(int id)
        {
            logger.LogInformation($"Recieved credit get request by id {id}");

            var request = await requestRepository.GetById(id);
            request.Applicant = await applicantRepository.GetById(request.Applicant.Id);
            request.RequestedCredit = await contractRepository.GetById(request.RequestedCredit.Id);
            return request;
        }

        public async Task<IEnumerable<CreditRequest>> GetByScoringStatus(bool? scoringStatus)
        {
            logger.LogInformation($"Recieved credit scoring status request by status {scoringStatus}");

            var requestsId = await requestRepository.GetIdByScoringStatus(scoringStatus);
            List<CreditRequest> result = new List<CreditRequest>();
            foreach(int id in requestsId)
            {
                var request = await GetById(id);
                result.Add(request);
            }
            return result;
        }

        public async Task<IEnumerable<CreditRequestEvaluateRequest>> GetForEvaluateRequest()
        {
            logger.LogInformation($"Recieved credit get for evaluated request");

            var result = new List<CreditRequestEvaluateRequest>();
            var requests = await GetByScoringStatus(null);
            foreach(var request in requests)
            {
                result.Add(mapper.Map<CreditRequestEvaluateRequest>(request));
            }
            return result;
        }

        public async Task<CreditRequestStatusResponse> GetScoringById(int id)
        {
            logger.LogInformation($"Recieved credit get scoring request by id {id}");

            var creditRequest = await requestRepository.GetById(id);

            creditRequest.Applicant = await applicantRepository.GetById(creditRequest.Applicant.Id);
            creditRequest.RequestedCredit = await contractRepository.GetById(creditRequest.RequestedCredit.Id);

            var result = mapper.Map<CreditRequestStatusResponse>(creditRequest);

            return result;
        }

        public async Task UpdateScoring(ScoringResultRequest item)
        {
            logger.LogInformation($"Recieved credit update scoring request for id {item.Id}");

            await requestRepository.UpdateScoring(item.ScoringStatus, item.ScoringDate, item.Id);
        }
    }
}
