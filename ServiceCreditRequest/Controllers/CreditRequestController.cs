using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceCreditRequest.Domain.Managers.Interfaces;
using ServiceCreditRequest.Models.Entities;
using ServiceCreditRequest.Models.Incoming.Request;
using ServiceCreditRequest.Models.Incoming.Response;
using ServiceCreditRequest.Models.ScoringResult.Request;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceCreditRequest.Controllers
{
    [Route("api/application")]
    [ApiController]
    public class CreditRequestController : ControllerBase
    {
        private ICreditRequestManager manager;
        private ILogger logger;
        public CreditRequestController(ICreditRequestManager manager, ILogger<CreditRequestController> logger)
        {
            this.manager = manager;
            this.logger = logger;
        }

        /// <summary>
        /// получение новой заявки на кредит
        /// </summary>
        /// <param name="creditRequest">заявка на кредит</param>
        /// <returns>int Id с котороым сохранена заявка</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreditRequestCreateRequest creditRequest)
        {
            int id = await manager.Create(creditRequest);

            logger.LogInformation($"Create request {id}");

            var result = new CreateResponse()
            {
                Id = id,
                ApplicationNum = creditRequest.ApplicationNum
            };

            return Ok(JsonSerializer.Serialize(result));
        }

        /// <summary>
        /// получение решения по заявке на кредит
        /// </summary>
        /// <param name="id">Id заявки на кредит</param>
        /// <returns></returns>
        [HttpGet("status/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            logger.LogInformation($"Status request {id}");

            var result = await manager.GetScoringById(id);

            return Ok(JsonSerializer.Serialize(result));
        }

        /// <summary>
        /// обновление результатов скоринга
        /// </summary>
        /// <param name="result">результат скоринга</param>
        /// <returns></returns>
        [HttpPut("scoring/update")]
        public async Task<IActionResult> UpdateScoring([FromBody] ScoringResultRequest result)
        {
            logger.LogInformation($"Update scoring {result.Id}");

            await manager.UpdateScoring(result);

            return Ok();
        }
    }
}
