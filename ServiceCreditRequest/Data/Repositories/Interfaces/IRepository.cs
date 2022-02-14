using System;
using System.Threading.Tasks;

namespace ServiceCreditRequest.Data.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        public Task<int> CreateAsync(T item);
        public Task<T> GetByIdAsync(int id);
    }
}
