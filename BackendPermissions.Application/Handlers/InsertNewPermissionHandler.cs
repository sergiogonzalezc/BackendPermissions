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
    public class InsertNewPermissionHandler : IRequestHandler<InsertNewPermissionCommand, ResultInsertPermissionDTO>
    {
        private readonly IPermissionsApplication _permissionsService;

        public InsertNewPermissionHandler(IPermissionsApplication permissionsApplication)
        {
            _permissionsService = permissionsApplication;
        }

        public async Task<ResultInsertPermissionDTO> Handle(InsertNewPermissionCommand request, CancellationToken cancellationToken)
        {
            return await _permissionsService.InsertNewPermission(request.input);
        }
    }
}

