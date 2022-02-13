using ServiceCreditRequest.Models.Entities;
using System.Threading.Tasks;

namespace CreditScoring.Data.Repository.Interfaces
{
    public interface IScoringRepository
    {
        /// <summary>
        /// Создание заявки на скоринг
        /// </summary>
        /// <param name="item">Входящая заявка на кредит</param>
        /// <returns></returns>
        public Task CreateAsync(CreditRequest item);
    }
}
