using BackendPermissions.Application.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPermissions.Application.Querys
{
    public record GetPermissionTypeByIdQuerys : IRequest<PermissionTypes>
    {
        public int id { get; }

        public GetPermissionTypeByIdQuerys(int id)
        {
            this.id = id;
        }
    }
}
