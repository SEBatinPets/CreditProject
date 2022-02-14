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
    /// репозиторий для обработки информации о получателе кредита
    /// </summary>
    public class CreditApplicantRepository : ICreditApplicantRepository
    {
        private readonly string connectionString;
        private readonly ILogger<CreditApplicantRepository> logger;
        public CreditApplicantRepository(
            IConfiguration configuration, 
            ILogger<CreditApplicantRepository> logger)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            this.logger = logger;
        }
        /// <summary>
        /// добавление в бд информации о получателе кредита
        /// </summary>
        /// <param name="item">информация о получателе кредита</param>
        /// <returns>id добавленного получателя в бд</returns>
        public async Task<int> CreateAsync(CreditApplicant item)
        {
            await using (var connection = new SqlConnection(connectionString))
            {
                int id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO credit_applicants(" +
                    "first_name, " +
                    "middle_name, " +
                    "last_name, " +
                    "date_birth, " +
                    "city_birth, " +
                    "adress_birth, " +
                    "adress_current, " +
                    "inn, " +
                    "snils, " +
                    "passport_num" +
                    ") " +

                    "OUTPUT Inserted.id " +

                    "VALUES (" +
                    "@first_name, " +
                    "@middle_name, " +
                    "@last_name, " +
                    "@date_birth, " +
                    "@city_birth, " +
                    "@adress_birth, " +
                    "@adress_current, " +
                    "@inn, " +
                    "@snils, " +
                    "@passport_num" +
                    ")", new
                    {
                        first_name = item.FirstName,
                        middle_name = item.MiddleName,
                        last_name = item.LastName,
                        date_birth = item.DateBirth,
                        city_birth = item.CityBirth,
                        adress_birth = item.AddressBirth,
                        adress_current = item.AddressCurrent,
                        inn = item.INN,
                        snils = item.SNILS,
                        passport_num = item.PassportNum
                    });

                logger.LogInformation($"Create request applicant id {id}");

                return id;
            }
        }

        /// <summary>
        /// получение информации о заемщике по id
        /// </summary>
        /// <param name="id">id заемщика</param>
        /// <returns>информация о получателе кредита</returns>
        public async Task<CreditApplicant> GetByIdAsync(int id)
        {
            logger.LogInformation($"Get request applicant by id{id}");
            await using (var connection = new SqlConnection(connectionString))
            {
                CreditApplicant result = await connection.QuerySingleOrDefaultAsync<CreditApplicant>
                    ("SELECT " +
                    "id as Id, " +
                    "first_name as FirstName, " +
                    "middle_name as MiddleName, " +
                    "last_name as LastName, " +
                    "date_birth as DateBirth, " +
                    "city_birth as CityBirth, " +
                    "adress_birth as AddressBirth, " +
                    "adress_current as AddressCurrent, " +
                    "inn as INN, " +
                    "snils as SNILS, " +
                    "passport_num as PassportNum " +
                    "FROM credit_applicants WHERE id=@id", new
                    {
                        id = id
                    });

                return result;
            }
        }
    }
}
