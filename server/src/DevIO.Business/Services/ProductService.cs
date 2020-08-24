using DevIO.Business.Interfaces;
using DevIO.Business.Model;
using DevIO.Business.Model.Validations;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUser _user;
        public ProductService(
            IProductRepository productRepository,
            INotifier notifier,
            IUser user
            ) : base(notifier)
        {
            _user = user;
            _productRepository = productRepository;
        }

        public async Task<Product> Add(Product product)
        {
            if (!ExecuteValidation(new ProductValidation(), product)) return null;

            // Possivel interagir com o usuario logado em qualquer camada (sem a implementacao so daria em controllers)
            //var user = _user.GetUserId();

            await _productRepository.Add(product);
            
            return product;
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
        }

        public async Task Remove(Guid productId)
        {
            await _productRepository.Remove(productId);
        }

        public async Task<Product> Update(Product product)
        {
            if (!ExecuteValidation(new ProductValidation(), product)) return null;

            await _productRepository.Update(product);

            return product;
        }
    }
}
