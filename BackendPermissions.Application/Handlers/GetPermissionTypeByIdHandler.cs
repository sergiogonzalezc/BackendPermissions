using BackendPermissions.Application.Interface;
using BackendPermissions.Application.Model;
using BackendPermissions.Application.Querys;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPermissions.Application.Handlers
{
    public class GetPermissionTypeByIdHandler : IRequestHandler<GetPermissionTypeByIdQuerys, PermissionTypes>
    {
        private readonly IPermissionsApplication _permissionsService;

        public GetPermissionTypeByIdHandler(IPermissionsApplication permissionsApplication)
        {
            _permissionsService = permissionsApplication;
        }

        public async Task<PermissionTypes> Handle(GetPermissionTypeByIdQuerys request, CancellationToken cancellationToken)
        {
            return await _permissionsService.GetPermissionTypeById(request.id);
        }
    }
}