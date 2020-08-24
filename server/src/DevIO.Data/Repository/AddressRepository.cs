using DevIO.Business.Interfaces;
using DevIO.Business.Model;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(MyDbContext context) : base(context) { }

        public async Task<Address> GetAddressByProvider(Guid providerId)
        {
            return await _context.Addresses.AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProviderId == providerId);
        }
    }
}
