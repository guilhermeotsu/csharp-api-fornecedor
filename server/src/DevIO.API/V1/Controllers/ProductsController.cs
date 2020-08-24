using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.API.Controllers;
using DevIO.API.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.API.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsController : MainController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly string _imgPrefix = $"{DateTime.Now.ToString("yyyy-MM-ddTHH'h'mm'min'ss'seg'")}_";
        public ProductsController(
            IProductRepository productRepository,
            IProductService productService,
            INotifier notifier,
            IMapper mapper,
            IUser user
        ) : base(notifier, user)
        {
            _productRepository = productRepository;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetAll()
        {
            var productViewModel = _mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsProviders());

            return CustomResponse(productViewModel);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductViewModel>> GetById(Guid id)
        {
            var product = await SearchProductById(id);

            if (product == null) return NotFound();

            return CustomResponse(ProductToProductViewModel(product));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProductViewModel>> Delete(Guid id)
        {
            var product = await SearchProductById(id);

            if (product == null) return NotFound();

            await _productService.Remove(id);

            return CustomResponse(ProductToProductViewModel(product));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductViewModel>> Update(
            Guid id,
            ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                NotifierError("Id not equal!");
                return CustomResponse();
            }

            var productUpdate = await GetById(id);
            productViewModel.Image = productUpdate.Value.Image;

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (productViewModel.ImageUpload != null)
            {
                var imageName = _imgPrefix + productViewModel.ImageUpload.FileName;

                if (!await UploadStream(productViewModel.ImageUpload, _imgPrefix))
                    return CustomResponse(ModelState);

                productUpdate.Value.Image = imageName;
            }

            productUpdate.Value.Name = productViewModel.Name;
            productUpdate.Value.Description = productViewModel.Description;
            productUpdate.Value.Value = productViewModel.Value;
            productUpdate.Value.Active = productViewModel.Active;

            await _productService.Update(_mapper.Map<Product>(productUpdate));

            return CustomResponse(productViewModel);
        }

        [RequestSizeLimit(40000000)]
        [HttpPost]
        public async Task<ActionResult<ProductViewModel>> AddImageLarge(
            ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (!await UploadStream(productViewModel.ImageUpload, _imgPrefix))
                return CustomResponse(productViewModel);

            productViewModel.Image = _imgPrefix + productViewModel.ImageUpload.FileName;

            await _productService.Add(_mapper.Map<Product>(productViewModel));

            return CustomResponse(productViewModel);
        }

        /*
          Recebe IFormFile e grava no servidor com stream (melhor opcao)
        */
        private async Task<bool> UploadStream(IFormFile file, string prefix)
        {
            if (file == null || file.Length == 0)
            {
                NotifierError("Image is empty!");
                return false;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imagens", $"{prefix}{file.FileName}");

            if (System.IO.File.Exists(filePath))
            {
                NotifierError("File name already exists!");
                return false;
            }

            using (var stream = new FileStream($"wwwroot/images/{prefix}{file.FileName}", FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;
        }

        private async Task<Product> SearchProductById(Guid id) => await _productRepository.GetById(id);

        private ProductViewModel ProductToProductViewModel(Product product) => _mapper.Map<ProductViewModel>(product);

        private Product ProductViewModelToProduct(ProductViewModel productViewModel) => _mapper.Map<Product>(productViewModel);
    }
}