using DevIO.Business.Interfaces;
using DevIO.Business.Model;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class ProviderRepository : Repository<Provider>, IProviderRepository
    {
        public ProviderRepository(MyDbContext context) : base(context) { }
        public async Task<Provider> GetProviderAddress(Guid id)
        {
            return await _context.Providers.AsNoTracking()
                .Include(p => p.Address)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Provider> GetProviderProductAddress(Guid id)
        {
            return await _context.Providers.AsNoTracking()
                .Include(o => o.Products)
                .Include(p => p.Address)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Provider>> GetProviderProductAddress()
        {
            return await _context.Providers.AsNoTracking()
                .Include(o => o.Products)
                .Include(p => p.Address)
                .ToListAsync();

        }
    }
}
