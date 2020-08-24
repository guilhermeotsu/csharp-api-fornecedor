using AutoMapper;
using DevIO.API.Controllers;
using DevIO.API.Extensions;
using DevIO.API.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DevIO.API.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/providers")]
    public class ProvidersController : MainController
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IProviderService _providerService;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        public ProvidersController(
            IProviderRepository repository,
            IProviderService service,
            IAddressRepository address,
            IMapper mapper,
            INotifier notifier,
            IUser user) : base(notifier, user)
        {
            _providerRepository = repository;
            _providerService = service;
            _mapper = mapper;
            _addressRepository = address;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProviderViewModel>>> GetAll()
            => Ok(_mapper.Map<IEnumerable<ProviderViewModel>>(await _providerRepository.GetProviderProductAddress()));

        [HttpGet("{providerId:guid}")]
        public async Task<ActionResult<ProviderViewModel>> GetById(Guid providerId)
        {
            var provider = await GetProviderProductsAdress(providerId);

            if (provider == null)
                return NotFound();

            return Ok(provider);
        }

        [HttpGet("get-address/{addressId:guid}")]
        public async Task<ActionResult<AddressViewModel>> GetAddress(Guid addressId)
        {
            var addressViewModel = _mapper.Map<AddressViewModel>(await _addressRepository.GetById(addressId));

            return CustomResponse(addressViewModel);
        }

        [ClaimsAuthorize("Administrator", "Update")]
        [HttpPut("update-address/{addressId:guid}")]
        public async Task<ActionResult<AddressViewModel>> UpdateAddress(Guid addressId, AddressViewModel addressViewModel)
        {
            if (addressId != addressViewModel.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return CustomResponse(addressViewModel);

            var address = _mapper.Map<Address>(addressViewModel);
            await _providerService.UpdateAddress(address);

            return CustomResponse(addressViewModel);
        }

        [ClaimsAuthorize("Administrator", "Create")]
        [HttpPost]
        public async Task<ActionResult<ProviderViewModel>> AddProvider(ProviderViewModel providerViewModel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var provider = _mapper.Map<Provider>(providerViewModel);
            await _providerService.Add(provider);

            return CustomResponse(providerViewModel);
        }

        [ClaimsAuthorize("Administrator", "Update")]
        [HttpPut("{providerId:guid}")]
        public async Task<ActionResult<ProviderViewModel>> UpdateProvider(Guid providerId, ProviderViewModel providerViewModel)
        {
            if (!ModelState.IsValid ||
                providerViewModel.Id != providerId)
                return BadRequest(ModelState);

            var provider = _mapper.Map<Provider>(providerViewModel);
            await _providerService.Update(provider);

            return CustomResponse(providerViewModel);
        }

        [ClaimsAuthorize("Administrator", "Delete")]
        [HttpDelete("{providerId:guid}")]
        public async Task<ActionResult<ProviderViewModel>> DeleteProvider(Guid providerId)
        {
            var providerViewModel = await GetProviderAddress(providerId);

            if (providerViewModel == null)
                return NotFound();

            await _providerService.Remove(providerId);

            return CustomResponse();
        }

        private async Task<ProviderViewModel> GetProviderProductsAdress(Guid providerId)
            => _mapper.Map<ProviderViewModel>(await _providerRepository.GetProviderProductAddress(providerId));

        private async Task<ProviderViewModel> GetProviderAddress(Guid providerId)
            => _mapper.Map<ProviderViewModel>(await _providerRepository.GetProviderAddress(providerId));
    }
}
