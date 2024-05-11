using AutoMapper;
using Restaurant.BusinessLogic.Implementation.Tables.Models;
using Restaurant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Tables.Mappings;

public class TableProfile : Profile
{
    public TableProfile()
    {
        CreateMap<TableModel, Table>()
            .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()));
    }
}
