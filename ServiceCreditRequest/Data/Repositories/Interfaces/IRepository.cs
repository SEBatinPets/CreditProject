using System;
using System.Threading.Tasks;

namespace ServiceCreditRequest.Data.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        public Task<int> Create(T item);
        public Task<T> GetById(int id);
    }
}
