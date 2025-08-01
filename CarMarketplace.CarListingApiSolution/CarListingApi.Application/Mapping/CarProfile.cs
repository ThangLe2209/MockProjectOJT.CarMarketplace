using AutoMapper;
using CarListingApi.Application.DTOs;
using CarListingApi.Domain.Entities;

namespace CarListingApi.Application.Mapping
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<CarInputDto, CarListing>();
            CreateMap<CarListing, CarListingDto>();
            CreateMap<CarListingDto, CarListing>();

            //Example
            // map from Image (entity) to Image, and back
            //CreateMap<Entities.Image, Model.Image>().ReverseMap();

            // map from ImageForCreation to Image
            // Ignore properties that shouldn't be mapped
            //CreateMap<Model.ImageForCreation, Entities.Image>()
            //	.ForMember(m => m.FileName, options => options.Ignore())
            //	.ForMember(m => m.Id, options => options.Ignore())
            //	.ForMember(m => m.OwnerId, options => options.Ignore());
        }
    }
}
