using DevIO.Business.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IProviderRepository : IRepository<Provider>
    {
        Task<Provider> GetProviderAddress(Guid id);
        Task<Provider> GetProviderProductAddress(Guid id);
        Task<IEnumerable<Provider>> GetProviderProductAddress();
    }
}
