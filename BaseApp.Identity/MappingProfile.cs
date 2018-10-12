using System.Collections.Generic;
using AutoMapper;
using BaseApp.Identity.Model;
using BaseApp.Identity.ViewModels;
using Profile = AutoMapper.Profile;

namespace BaseApp.Identity
{
    public class MappingProfile : Profile {
        public MappingProfile() {
            // Add as many of these lines as you need to map your objects
            CreateMap<ApplicationUser, RegisterViewModel>();
            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForPath(d => d.ExternalData.Gender, s => s.MapFrom(src => src.Gender))
                .ForPath(d => d.ExternalData.Location, s => s.MapFrom(src => src.Location));



            CreateMap<ApplicationUser, ShowUsersViewModel>()
                .ForPath(d => d.Gender, s => s.MapFrom(src => src.ExternalData.Gender.ToString()))
                .ForPath(d => d.Location, s => s.MapFrom(src => src.ExternalData.Location))
                .ForPath(d => d.Locale, s => s.MapFrom(src => src.ExternalData.Locale))
                .ForPath(d => d.PhoneNumber, s => s.MapFrom(src => src.PhoneNumber))
                .ForPath(d => d.Email, s => s.MapFrom(src => src.Email))
                .ForPath(d => d.UserName, s => s.MapFrom(src => src.UserName));


            CreateMap<AddNewRoleViewModel, ApplicationRole>()
                .ForPath(d=>d.Name,s=>s.MapFrom(src=>src.RoleName))
                .ForPath(d=>d.NormalizedName,s=>s.MapFrom(src=>src.RoleName.Normalize()))
                ;
            CreateMap<ActionList, AccessAction>()
                .ForPath(d=>d.ActionName,s=>s.MapFrom(src=>src.ActionName))
                .ForPath(d=>d.ActionNameNormalized,s=>s.MapFrom(src=>src.ActionName.Normalize()))
                .ForPath(d=>d.ControlName,s=>s.MapFrom(src=>src.ControllerName))
                .ForPath(d=>d.ControllerNameNormalized,s=>s.MapFrom(src=>src.ControllerName.Normalize()))

                ;

        }
    }
}