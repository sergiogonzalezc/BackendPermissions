using BackendPermissions.Application.Commands;
using BackendPermissions.Application.Interface;
using BackendPermissions.Application.Querys;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPermissions.Application.Handlers
{
    /// <summary>
    /// Validate a permission by name and last name
    /// </summary>
    public class GetValidatePermissionHandler : IRequestHandler<GetValidatePermissionQuery, bool>
    {
        private readonly IPermissionsApplication _permissionsService;

        public GetValidatePermissionHandler(IPermissionsApplication permissionsApplication)
        {
            _permissionsService = permissionsApplication;
        }

        public async Task<bool> Handle(GetValidatePermissionQuery request, CancellationToken cancellationToken)
        {
            return await _permissionsService.ExistsPermissionByNameAndType(request.NombreEmpleado, request.ApellidoEmpleado);
        }

    }
}
