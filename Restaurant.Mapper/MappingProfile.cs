using AutoMapper;
using Restaurant.Models;
using RestaurantViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Mapper
{
   public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Booking, BookingVM>()
                .ForMember(dest => dest.TableId, opt => opt.MapFrom(src => src.Tables.Id))
                .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.ApplicationUserId))
                .ForMember(dest => dest.BookingTime, opt => opt.MapFrom(src => src.BookingTime.ToString()));
        }
    }
}
