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
				.ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()))
				.ForMember(a => a.Date, a => a.MapFrom(s => CombineDateTime(s.Date, s.Time)))
				.ForMember(a => a.UserId, a => a.Ignore());
        }

		private DateTime CombineDateTime(DateOnly date, string timeAsString)
		{
			var modelTimeParseResult = TimeOnly.TryParse(timeAsString, out var time);

			if (modelTimeParseResult == false)
			{
				throw new FormatException();
			}
			return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
		}
	}
}
