using AutoMapper;
using Contracts.DTOs.ProductCatalog;
using ProductCatalogService.Entities;

namespace ProductCatalogService.Profiles
{
    public class MappingProfile : Profile
    {
        protected MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryDto>().ReverseMap();
            CreateMap<ProductImage, ProductImageDto>().ReverseMap();
            CreateMap<ProductInventory, ProductInventoryDto>().ReverseMap();
            CreateMap<ProductTag, ProductTagDto>().ReverseMap();
            CreateMap<ProductTagRelation, ProductTagRelationDto>().ReverseMap();
            CreateMap<ProductVariant, ProductVariantDto>().ReverseMap();
        }
    }
}
