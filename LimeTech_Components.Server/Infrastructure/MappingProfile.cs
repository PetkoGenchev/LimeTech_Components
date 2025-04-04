namespace LimeTech_Components.Server.Infrastructure
{
    using AutoMapper;
    using LimeTech_Components.Server.Data.Models;
    using LimeTech_Components.Server.DTOs;
    using LimeTech_Components.Server.Services.Components.Models;
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Component, ComponentServiceModel>();

            this.CreateMap<BasketItem, BasketItemDTO>()
                .ForMember(dest => dest.ComponentName, opt => opt.MapFrom(src => src.Component.Name))
                .ForMember(dest => dest.PricePerUnit, opt => opt.MapFrom(src => src.Component.Price))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Quantity * src.Component.Price));

            this.CreateMap<Component, ComponentDTO>();

            this.CreateMap<BasketItem, PurchaseHistory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            this.CreateMap<PurchaseHistory, PurchaseHistoryDTO>();


        }
    }


}
