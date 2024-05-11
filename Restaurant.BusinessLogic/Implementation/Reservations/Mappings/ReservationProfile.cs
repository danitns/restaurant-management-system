using AutoMapper;
using Restaurant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Reservations.Mappings
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<CreateReservationModel, Reservation>()
                .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()));
        }
    }
}
