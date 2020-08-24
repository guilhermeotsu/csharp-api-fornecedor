using DevIO.Business.Interfaces;
using DevIO.Business.Model;
using DevIO.Business.Model.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Business.Services
{
    public class ProviderService : BaseService, IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IAddressRepository _addressRepository;

        public ProviderService(
            IProviderRepository providerRepository,
            IAddressRepository addressRepository,
            INotifier notifier
            ) : base(notifier)
        {
            _providerRepository = providerRepository;
            _addressRepository = addressRepository;
        }

        public async Task<bool> Add(Provider provider)
        {
            if (!ExecuteValidation(new ProviderValidation(), provider) ||
                !ExecuteValidation(new AddressValidation(), provider.Address)) return false;

            if(_providerRepository.Search(x => x.Document == provider.Document).Result.Any())
            {
                Notify("There is already a supplier with the document informed.");
                return false;
            }

            await _providerRepository.Add(provider);
            return true;
        }

        public void Dispose()
        {
            _providerRepository?.Dispose();
            _addressRepository?.Dispose();
        }

        public async Task<bool> Remove(Guid providerId)
        {
            if(_providerRepository.GetProviderProductAddress(providerId).Result.Products.Any())
            {
                Notify("The supplier does not have registered products");
                return false;
            }

            var address = await _addressRepository.GetAddressByProvider(providerId);

            if (address != null)
                await _addressRepository.Remove(address.Id);

            await _providerRepository.Remove(providerId);

            return true;
        }

        public async Task<bool> Update(Provider provider)
        {
            if (!ExecuteValidation(new ProviderValidation(), provider)) return false;

            var providerDb = await _providerRepository.Search(x => x.Id == provider.Id);

            if (!providerDb.First().Document.Equals(provider.Document))
            {
                if (_providerRepository.Search(x => x.Document == provider.Document).Result.Any())
                {
                    Notify("There is already a supplier with the document informed.");
                    return false;
                }
            }

            await _providerRepository.Update(provider);

            return true;
        }

        public async Task UpdateAddress(Address address)
        {
            if (!ExecuteValidation(new AddressValidation(), address)) return;

            await _addressRepository.Update(address);
        }
    }
}
