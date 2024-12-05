namespace LimeTech_Components.Server.Infrastructure
{
    using AutoMapper;
    using LimeTech_Components.Server.Data.Models;
    using LimeTech_Components.Server.Services.Components.Models;
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            this.CreateMap<Component, ComponentServiceModel>();
            this.CreateMap<Component,TopPurchasedComponentServiceModel>();
        }
    }
}
