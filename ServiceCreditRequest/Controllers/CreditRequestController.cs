using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceCreditRequest.Domain.Managers.Interfaces;
using ServiceCreditRequest.Models.Entities;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using CreditProjectRequestsModels.Models.CreateRequest.Request;
using CreditProjectRequestsModels.Models.CreateRequest.Response;
using CreditProjectRequestsModels.Models.ScoringResultRequest.Request;
using CreditProjectRequestsModels.Models.StatusRequest.Request;

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
        public async Task<IActionResult> CreateAsync([FromBody] CreditRequestCreateRequest creditRequest)
        {
            int id = await manager.CreateAsync(creditRequest);

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
        [HttpGet("status")]
        public async Task<IActionResult> GetByIdAsync([FromBody] StatusRequest statusRequest)
        {
            logger.LogInformation($"Status request {statusRequest.Id}");

            var result = await manager.GetScoringByIdAsync(statusRequest.Id);

            return Ok(JsonSerializer.Serialize(result));
        }

        /// <summary>
        /// обновление результатов скоринга
        /// </summary>
        /// <param name="result">результат скоринга</param>
        /// <returns></returns>
        [HttpPut("scoring/update")]
        public async Task<IActionResult> UpdateScoringAsync([FromBody] ScoringResultRequest result)
        {
            logger.LogInformation($"Update scoring {result.Id}");

            await manager.UpdateScoringAsync(result);

            return Ok();
        }
    }
}
