using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Domain.Entities;
using AutoMapper;

namespace ToDoApp.Application.Mapping
{
    public class UserClaimProfile : Profile
    {
        public UserClaimProfile()
        {
            CreateMap<UserClaim, UserClaimDto>();
            //CreateMap<CommentForCreationDto, CommentModel>();
            //CreateMap<CommentForUpdateDto, CommentModel>();



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
