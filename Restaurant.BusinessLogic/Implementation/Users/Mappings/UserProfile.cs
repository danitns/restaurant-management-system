using AutoMapper;
using Restaurant.Entities;
using Restaurant.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Users
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterModel, User>()
                .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()))
                .ForMember(a => a.RoleId, a => a.MapFrom(s => ((int)RoleTypes.User)))
                .ForMember(a => a.PasswordHash, a => a.MapFrom(s => Guid.NewGuid()))
                .ForMember(a => a.Birthdate, a => a.MapFrom(s => s.Birthdate));

            CreateMap<User, DetailsUserModel>();
        }
    }
}
