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
        public async Task<int> CreateAsync(CreditRequestCreateRequest item)
        {
            logger.LogInformation($"Recieved credit create request {item.ApplicationNum}");

            var creditRequest = mapper.Map<CreditRequest>(item);

            creditRequest.Applicant.Id = await applicantRepository.CreateAsync(creditRequest.Applicant);
            creditRequest.RequestedCredit.Id = await contractRepository.CreateAsync(creditRequest.RequestedCredit);

            var id = await requestRepository.CreateAsync(creditRequest);

            return id;
        }

        public async Task<CreditRequest> GetByIdAsync(int id)
        {
            logger.LogInformation($"Recieved credit get request by id {id}");

            var request = await requestRepository.GetByIdAsync(id);
            request.Applicant = await applicantRepository.GetByIdAsync(request.Applicant.Id);
            request.RequestedCredit = await contractRepository.GetByIdAsync(request.RequestedCredit.Id);
            return request;
        }

        public async Task<IEnumerable<CreditRequest>> GetByScoringStatusAsync(bool? scoringStatus)
        {
            logger.LogInformation($"Recieved credit scoring status request by status {scoringStatus}");

            var requestsId = await requestRepository.GetIdByScoringStatusAsync(scoringStatus);
            List<CreditRequest> result = new List<CreditRequest>();
            foreach(int id in requestsId)
            {
                var request = await GetByIdAsync(id);
                result.Add(request);
            }
            return result;
        }

        public async Task<IEnumerable<CreditRequestEvaluateRequest>> GetForEvaluateRequestAsync()
        {
            logger.LogInformation($"Recieved credit get for evaluated request");

            var result = new List<CreditRequestEvaluateRequest>();
            var requests = await GetByScoringStatusAsync(null);
            foreach(var request in requests)
            {
                result.Add(mapper.Map<CreditRequestEvaluateRequest>(request));
            }
            return result;
        }

        public async Task<CreditRequestStatusResponse> GetScoringByIdAsync(int id)
        {
            logger.LogInformation($"Recieved credit get scoring request by id {id}");

            var creditRequest = await requestRepository.GetByIdAsync(id);

            if(creditRequest == null)
            {
                return null;
            }

            creditRequest.Applicant = await applicantRepository.GetByIdAsync(creditRequest.Applicant.Id);
            creditRequest.RequestedCredit = await contractRepository.GetByIdAsync(creditRequest.RequestedCredit.Id);

            var result = mapper.Map<CreditRequestStatusResponse>(creditRequest);

            return result;
        }

        public async Task UpdateScoringAsync(ScoringResultRequest item)
        {
            logger.LogInformation($"Recieved credit update scoring request for id {item.Id}");

            await requestRepository.UpdateScoringAsync(item.ScoringStatus, item.ScoringDate, item.Id);
        }
    }
}
