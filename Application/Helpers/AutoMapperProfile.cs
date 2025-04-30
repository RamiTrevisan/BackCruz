using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Role, opt => opt.Ignore());
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.LastLogin, opt => opt.Ignore());

            // Customer mappings
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore());

            CreateMap<Order, OrderSummaryDto>()
                .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.OrderDetails.Count))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));

            // FoodItem mappings
            CreateMap<FoodItem, FoodItemDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<FoodItemDto, FoodItem>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.MenuItems, opt => opt.Ignore());

            // Menu mappings
            CreateMap<Menu, MenuDto>();
            CreateMap<MenuDto, Menu>()
                .ForMember(dest => dest.MenuItems, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore());

            CreateMap<MenuFoodItem, MenuFoodItemDto>()
                .ForMember(dest => dest.FoodItemName, opt => opt.MapFrom(src => src.FoodItem.Name))
                .ForMember(dest => dest.FoodItemPrice, opt => opt.MapFrom(src => src.FoodItem.Price));
            CreateMap<MenuFoodItemDto, MenuFoodItem>()
                .ForMember(dest => dest.Menu, opt => opt.Ignore())
                .ForMember(dest => dest.FoodItem, opt => opt.Ignore());

            // Order mappings
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(src => src.PaymentMethod.ToString()));
            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore());

            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.MenuName, opt => opt.MapFrom(src => src.Menu.Name));
            CreateMap<OrderDetailDto, OrderDetail>()
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Menu, opt => opt.Ignore());

            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => OrderStatus.Pending))
                .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<CreateOrderDetailDto, OrderDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Menu, opt => opt.Ignore())
                .ForMember(dest => dest.Subtotal, opt => opt.Ignore())
                .ForMember(dest => dest.UnitPrice, opt => opt.Ignore());

            // FoodCategory mappings
            CreateMap<FoodCategory, FoodCategoryDto>();
            CreateMap<FoodCategoryDto, FoodCategory>()
                .ForMember(dest => dest.FoodItems, opt => opt.Ignore());

            // Role mappings
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>()
                .ForMember(dest => dest.Users, opt => opt.Ignore());

            // Customer creation mapping
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // FoodCategory creation mapping
            CreateMap<CreateFoodCategoryDto, FoodCategory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FoodItems, opt => opt.Ignore());

            // FoodItem creation mapping
            CreateMap<CreateFoodItemDto, FoodItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.MenuItems, opt => opt.Ignore());

            // Menu creation mapping
            CreateMap<CreateMenuDto, Menu>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MenuItems, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore());

            // Add Menu Item mapping
            CreateMap<AddMenuItemDto, MenuFoodItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Menu, opt => opt.Ignore())
                .ForMember(dest => dest.FoodItem, opt => opt.Ignore());
        }
    }
}