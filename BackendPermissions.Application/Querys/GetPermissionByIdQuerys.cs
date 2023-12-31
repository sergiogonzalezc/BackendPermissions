﻿using BackendPermissions.Application.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPermissions.Application.Querys
{
    public class GetPermissionByIdQuerys : IRequest<PermissionsDTO>
    {
        public int id { get; }

        public GetPermissionByIdQuerys(int id)
        {
            this.id = id;
        }

    }
}
