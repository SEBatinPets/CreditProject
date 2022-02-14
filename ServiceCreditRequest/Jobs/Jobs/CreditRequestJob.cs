using Microsoft.Extensions.Logging;
using Quartz;
using ServiceCreditRequest.Client;
using ServiceCreditRequest.Domain.Managers.Interfaces;
using System;
using System.Threading.Tasks;

namespace ServiceCreditRequest.Jobs
{
    public class CreditRequestJob : IJob
    {
        private readonly ICreditRequestManager requestManager;
        private readonly ILogger<CreditRequestJob> logger;
        private readonly ICreditRequestClient client;
        public CreditRequestJob(
            ILogger<CreditRequestJob> logger,
            ICreditRequestManager requestManager,
            ICreditRequestClient client)
        {
            this.logger = logger;
            this.requestManager = requestManager;
            this.client = client;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            logger.LogInformation($"Start credit request job");

            var requests = await requestManager.GetForEvaluateRequestAsync();

            foreach (var request in requests)
            {
                await client.SendEvaluateRequestAsync(request);
            }
        }
    }
}
