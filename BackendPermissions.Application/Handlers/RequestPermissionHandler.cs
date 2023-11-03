using BackendPermissions.Application.Commands;
using BackendPermissions.Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPermissions.Application.Handlers
{
    public class RequestPermissionHandler : IRequestHandler<InsertPermissionCommand, bool>
    {
        private readonly IPermissionsApplication _permissionsService;

        public RequestPermissionHandler(IPermissionsApplication permissionsApplication)
        {
            _permissionsService = permissionsApplication;
        }

        public async Task<bool> Handle(InsertPermissionCommand request, CancellationToken cancellationToken)
        {
            return await _permissionsService.ExistsPermissionByNameAndType(request.NombreEmpleado, request.ApellidoEmpleado);
        }

    }
}
