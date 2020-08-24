using DevIO.Business.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IProductService : IDisposable
    {
        Task<Product> Add(Product product);
        Task<Product> Update(Product product);
        Task Remove(Guid productId);
    }
}
