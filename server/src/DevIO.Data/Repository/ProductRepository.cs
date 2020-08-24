using DevIO.Business.Interfaces;
using DevIO.Business.Model;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MyDbContext context) : base(context) { }
        public async Task<IEnumerable<Product>> GetProductByProvider(Guid providerId)
        {
            return await Search(p => p.ProviderId == providerId);
        }
        public async Task<IEnumerable<Product>> GetProductsProviders()
        {
            return await _context.Products.AsNoTracking()
                .Include(p => p.Provider)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Product> GetProductProvider(Guid providerId)
        {
            return await _context.Products.AsNoTracking()
                .Include(m => m.Provider)
                .FirstOrDefaultAsync(x => x.Id == providerId);
        }

    }
}
