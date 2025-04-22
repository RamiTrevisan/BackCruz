using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.LastLogin, opt => opt.Ignore());

            // Customer mappings
            CreateMap<Customer, CustomerDto>();
            CreateMap<Order, OrderSummaryDto>()
                .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.OrderDetails.Count))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));

            // FoodItem mappings
            CreateMap<FoodItem, FoodItemDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            // Menu mappings
            CreateMap<Menu, MenuDto>();
            CreateMap<MenuFoodItem, MenuFoodItemDto>()
                .ForMember(dest => dest.FoodItemName, opt => opt.MapFrom(src => src.FoodItem.Name))
                .ForMember(dest => dest.FoodItemPrice, opt => opt.MapFrom(src => src.FoodItem.Price));

            // Order mappings
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(src => src.PaymentMethod.ToString()));

            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.MenuName, opt => opt.MapFrom(src => src.Menu.Name));

            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => OrderStatus.Pending))
                .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => false));

            CreateMap<CreateOrderDetailDto, OrderDetail>();

            // FoodCategory mappings
            CreateMap<FoodCategory, FoodCategoryDto>();
        }
    }
}