using DevIO.Business.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IProviderService : IDisposable
    {
        Task<bool> Add(Provider provider);
        Task<bool> Update(Provider provider);
        Task<bool> Remove(Guid providerId);
        Task UpdateAddress(Address address);
    }
}
