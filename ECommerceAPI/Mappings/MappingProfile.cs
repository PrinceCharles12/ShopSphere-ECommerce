using AutoMapper;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;

namespace ECommerceAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product Mappings
            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.CategoryId,opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null
                    ? src.Category.Name: string.Empty));

            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();

            // Cart Mappings
            CreateMap<CartItem, CartItemResponseDto>()
                .ForMember(
                    dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                .ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0))
                .ForMember(
                    dest => dest.TotalPrice,
                    opt => opt.MapFrom(src => src.Product != null ? src.Product.Price * src.Quantity : 0));


            // Order Mappings
            CreateMap<Order, OrderResponseDto>();

            // Order Item Mappings
            CreateMap<OrderItem, OrderItemResponseDto>()
                .ForMember(
                    dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty));
            
            CreateMap<Payment, PaymentResponseDto>();
        }
    }
}