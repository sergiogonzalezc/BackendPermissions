using BackendPermissions.Application.Commands;
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
    public class GetPermissionTypesHandler : IRequestHandler<GetPermissionTypesQuerys, List<PermissionTypes>>
    {
        private readonly IPermissionsApplication _permissionsService;

        public GetPermissionTypesHandler(IPermissionsApplication permissionsApplication)
        {
            _permissionsService = permissionsApplication;
        }

        public async Task<List<PermissionTypes>> Handle(GetPermissionTypesQuerys request, CancellationToken cancellationToken)
        {
            return await _permissionsService.GetPermissionTypes();
        }
    }
}