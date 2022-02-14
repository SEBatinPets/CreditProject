using Microsoft.Extensions.Configuration;
using ServiceCreditRequest.Data.Repositories.Interfaces;
using ServiceCreditRequest.Models.Entities;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ServiceCreditRequest.Data.Repositories.Implementation
{
    /// <summary>
    /// репозиторий обработки заявок на кредит
    /// </summary>
    public class CreditRequestRepository : ICreditRequestRepository
    {
        private readonly string connectionString;
        private readonly ILogger<CreditRequestRepository> logger;
        public CreditRequestRepository(
            IConfiguration configuration,
            ILogger<CreditRequestRepository> logger) 
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            this.logger = logger;
        }
        /// <summary>
        /// запись в бд новой заявки на кредит
        /// </summary>
        /// <param name="item">новая заявка на кредит</param>
        /// <returns>id заявки на кредит</returns>
        public async Task<int> Create(CreditRequest item)
        {
            logger.LogInformation($"Create request applicant {item.ApplicationNum}");

            //проверка что договор существует
            var exist = await GetByApplicationNum(item.ApplicationNum);
            if (exist != null)
            {
                return exist.Id;
            }

            await using(var connection = new SqlConnection(connectionString))
            {
                int id = await connection.QuerySingleAsync<int>("INSERT INTO credit_requests(" +
                    "application_num, " +
                    "application_date, " +
                    "branch_bank, " +
                    "branch_bank_addr, " +
                    "credit_manager_id, " +
                    "credit_applicant_id, " +
                    "credit_contract_id" +
                    ") " +

                    "OUTPUT Inserted.id " +

                    "VALUES (" +
                    "@application_num, " +
                    "@application_date, " +
                    "@branch_bank, " +
                    "@branch_bank_addr, " +
                    "@credit_manager_id, " +
                    "@credit_applicant_id, " +
                    "@credit_contract_id" +
                    ")", new
                    {
                        application_num = item.ApplicationNum,
                        application_date = item.ApplicationDate,
                        branch_bank = item.BranchBank,
                        branch_bank_addr = item.BranchBankAddr,
                        credit_manager_id = item.CreditManagerId,
                        credit_applicant_id = item.Applicant.Id,
                        credit_contract_id = item.RequestedCredit.Id
                    });
                return id;
            }
        }

        /// <summary>
        /// получение заявки на кредит по id
        /// </summary>
        /// <param name="id">id заявки на кредит</param>
        /// <returns>заявка на кредит</returns>
        public async Task<CreditRequest> GetById(int id)
        {
            logger.LogInformation($"Get request by id {id}");
            await using (var connection = new SqlConnection(connectionString))
            {
                CreditRequest result = await connection.QuerySingleOrDefaultAsync<CreditRequest>
                    ("SELECT " +
                    "id as Id, " +
                    "application_num as ApplicationNum, " +
                    "application_date as ApplicationDate, " +
                    "branch_bank as BranchBank, " +
                    "branch_bank_addr as BranchBankAddr, " +
                    "credit_manager_id as CreditManagerId, " +
                    "scoring_status as ScoringStatus, " +
                    "scoring_date as ScoringDate " +
                    "FROM credit_requests WHERE id=@id",
                    new
                    {
                        id = id
                    });

                if(result == null)
                {
                    return null;
                }

                
                int creditApplicantId = await connection.QuerySingleOrDefaultAsync<int>
                    ("SELECT credit_applicant_id FROM credit_requests WHERE id=@id",
                    new
                    {
                        id = id
                    });

                //создание applicant заглушки
                result.Applicant = new CreditApplicant()
                {
                    Id = creditApplicantId
                };

                int creditContractId = await connection.QuerySingleOrDefaultAsync<int>
                    ("SELECT credit_contract_id FROM credit_requests WHERE id=@id",
                    new
                    {
                        id = id
                    });

                //создание contract заглушки
                result.RequestedCredit = new CreditContract()
                {
                    Id = creditApplicantId
                };

                return result;
            }
        }

        /// <summary>
        /// получение Id запросов по заданному статусу скоринга
        /// </summary>
        /// <param name="scoringResult"></param>
        /// <returns></returns>
        public async Task<IEnumerable<int>> GetIdByScoringStatus(bool? scoringResult)
        {
            logger.LogInformation($"Get requests by scoring result {scoringResult}");
            if (scoringResult == null)
            {
                return await GetByNullScoring();
            }
            else
            {
                return await GetByBoolScoring(scoringResult.Value);
            }
        }

        private async Task<IEnumerable<int>> GetByBoolScoring(bool scoringResult)
        {
            await using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<int>
                    ("SELECT id FROM credit_requests WHERE scoring_status=@scoring_result", new
                    {
                        scoring_result = scoringResult
                    });
            }
        }

        private async Task<IEnumerable<int>> GetByNullScoring()
        {
            await using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<int>
                    ("SELECT id FROM credit_requests WHERE scoring_status is null");
            }
        }

        public async Task UpdateScoring(bool scoringResult, DateTime scoringDate, int id)
        {
            logger.LogInformation($"Update scoring result request by id {id}");
            await using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(
                    "UPDATE credit_requests SET " +
                    "scoring_status = @scoring_status, " +
                    "scoring_date = @scoring_date " +
                    "WHERE id = @id", new
                    {
                        scoring_status = scoringResult,
                        scoring_date = scoringDate,
                        id = id
                    });
            }
        }

        public async Task<CreditRequest> GetByApplicationNum(string applicantNum)
        {
            logger.LogInformation($"Get request by applicant num {applicantNum}");
            int? requestId;
            await using (var connection = new SqlConnection(connectionString))
            {
                requestId = await connection.QuerySingleOrDefaultAsync<int?>(
                    "SELECT id FROM credit_requests WHERE application_num=@application_num",
                    new
                    {
                        application_num = applicantNum
                    });
            }

            if(requestId != null)
            {
                return await GetById(requestId.Value);
            } else
            {
                return null;
            }

        }
    }
}
