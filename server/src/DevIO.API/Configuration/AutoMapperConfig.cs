using AutoMapper;
using DevIO.API.ViewModels;
using DevIO.Business.Model;

namespace DevIO.API.Configuration
{
    // Configurando De -> Para do AutoMapper
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            // Mapenado de Provider para ProviderViewModel
            // e como ela tem o mesmo contrutor poder fazer de ProviderViewModel para Provider com o ReverseMap
            CreateMap<Provider, ProviderViewModel>().ReverseMap();
            CreateMap<Address, AddressViewModel>().ReverseMap();
            CreateMap<ProductViewModel, Product>().ReverseMap();

            // Mapeamento do nome do fornecedor
            CreateMap<Product, ProductViewModel>()
            .ForMember(dest => dest.NameProvider, 
                opt => opt.MapFrom(src => src.Provider.Name)
            );
        }
    }
}
