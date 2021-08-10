using AutoMapper;
using DTS_DogBarber_Shop.Data.Models.Dtos;

namespace DTS_DogBarber_Shop.Data.Models.Profiles
{
    public class AppoinmentProfile : Profile
    {
        public AppoinmentProfile()
        {
            CreateMap<AppointmentIdentity, AppointmentIdentityDto>();
            CreateMap<AppointmentIdentityDto, AppointmentIdentity>();
            CreateMap<AppointmentIdentity, CreateNewQueueDto>();
            CreateMap<CreateNewQueueDto, AppointmentIdentity>();
        }
    }
}
