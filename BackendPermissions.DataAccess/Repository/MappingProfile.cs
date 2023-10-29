using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BackendPermissions.Application.Model;
using BackendPermissions.Infraestructura.Repositor;

namespace BackendPermissions.Infraestructura.Repository
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PermissionsEF, Permissions>().ReverseMap();
            CreateMap<PermissionTypesEF, PermissionTypes>().ReverseMap();
        }       
    }
}
