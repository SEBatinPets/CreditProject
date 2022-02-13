using CreditProjectRequestsModels.Models.EvaluateRequest.Request;
using CreditScoring.Domain.Managers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CreditScoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoringController : ControllerBase
    {
        private readonly ICreditScoringManager scoringManager;
        private readonly ILogger<ScoringController> logger;
        public ScoringController(ICreditScoringManager scoringManager, ILogger<ScoringController> logger)
        {
            this.scoringManager = scoringManager;
            this.logger = logger;
        }
        /// <summary>
        /// Добавление новой заявки на кредит для скоринга
        /// </summary>
        /// <param name="creditRequest">заявка на кредит</param>
        /// <returns></returns>
        /// <response code="200">Если всё хорошо</response>
        [HttpPost("evaluate")]
        public Task<IActionResult> Scoring([FromBody] CreditRequestEvaluateRequest creditRequest)
        {
            logger.LogInformation($"{DateTime.Now}Scoring request by application {creditRequest.ApplicationNum}");

            //асинхронная обработка заявки
            Task.Run(() => scoringManager.ScoringAsync(creditRequest));

            return Task.FromResult<IActionResult>(Ok());
        }
    }
}
