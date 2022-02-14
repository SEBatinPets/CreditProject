using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceCreditRequest.Data.Repositories.Interfaces;
using ServiceCreditRequest.Models.Entities;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ServiceCreditRequest.Data.Repositories.Implementation
{
    /// <summary>
    /// репозиторий для обработки информации о кредитном продукте
    /// </summary>
    public class CreditContractRepository : ICreditContractRepository
    {
        private readonly string connectionString;
        private readonly ILogger<CreditContractRepository> logger;
        public CreditContractRepository(
            IConfiguration configuration, 
            ILogger<CreditContractRepository> logger)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            this.logger = logger;
        }
        /// <summary>
        /// добавление кредита в бд
        /// </summary>
        /// <param name="item">информация о кредите</param>
        /// <returns>id кредита</returns>
        public async Task<int> CreateAsync(CreditContract item)
        {
            var exist = await Exist(item);
            if (exist != null)
            {
                return exist.Id;
            }
            await using (var connection = new SqlConnection(connectionString))
            {
                int id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO credit_contracts(" +
                    "credit_type, " +
                    "requested_amount, " +
                    "requested_currency, " +
                    "annual_salary, " +
                    "month_salary, " +
                    "company_name, " +
                    "comment" +
                    ") " +

                    "OUTPUT Inserted.id " +
                    
                    "VALUES (" +
                    "@credit_type, " +
                    "@requested_amount, " +
                    "@requested_currency, " +
                    "@annual_salary, " +
                    "@month_salary, " +
                    "@company_name, " +
                    "@comment" +
                    ")", new
                    {
                        credit_type = item.CreditType,
                        requested_amount = item.RequestedAmount,
                        requested_currency = item.RequestedCurrency,
                        annual_salary = item.AnnualSalary,
                        month_salary = item.MonthlySalary,
                        company_name = item.CompanyName,
                        comment = item.Comment
                    });

                logger.LogInformation($"Create request contract id {id}");

                return id;
            }
        }

        /// <summary>
        /// получение информации о кредите по id
        /// </summary>
        /// <param name="id">id кредита</param>
        /// <returns>кредит</returns>
        public async Task<CreditContract> GetByIdAsync(int id)
        {
            logger.LogInformation($"Get request contract by id {id}");
            await using (var connection = new SqlConnection(connectionString))
            {
                CreditContract result = await connection.QuerySingleOrDefaultAsync<CreditContract>(
                    "SELECT " +
                    "id as Id, " +
                    "credit_type as CreditType, " +
                    "requested_amount as RequestedAmount, " +
                    "requested_currency as RequestedCurrency, " +
                    "annual_salary as AnnualSalary, " +
                    "month_salary as MonthlySalary, " +
                    "company_name as CompanyName, " +
                    "comment as Comment " +
                    "FROM credit_contracts WHERE id=@id", new
                    {
                        id = id
                    });

                return result;
            }
        }
        private async Task<CreditContract> Exist(CreditContract item)
        {
            int? requestId;
            await using (var connection = new SqlConnection(connectionString))
            {
                requestId = await connection.QuerySingleOrDefaultAsync<int?>(
                    "SELECT id " +
                    "FROM credit_contracts " +
                    "WHERE " +
                    "credit_type=@credit_type AND " +
                    "requested_amount=@requested_amount AND " +
                    "requested_currency=@requested_currency AND " +
                    "annual_salary=@annual_salary AND " +
                    "month_salary=@month_salary AND " +
                    "company_name=@company_name AND " +
                    "comment=@comment", 
                    new
                    {
                        credit_type = item.CreditType,
                        requested_amount = item.RequestedAmount,
                        requested_currency = item.RequestedCurrency,
                        annual_salary = item.AnnualSalary,
                        month_salary = item.MonthlySalary,
                        company_name = item.CompanyName,
                        comment = item.Comment
                    });
            }
            if(requestId != null)
            {
                return await GetByIdAsync(requestId.Value);
            }
            else
            {
                return null;
            }
        }
    }
}
