using AutoMapper;
using VacationRental.Common.Entities;
using VacationRental.Common.Models;

namespace VacationRental.Service.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookingEntity, BookingViewModel>();
            
            CreateMap<BookingBindingModel, BookingEntity>();

            CreateMap<RentalEntity, RentalViewModel>();
        }
    }
}