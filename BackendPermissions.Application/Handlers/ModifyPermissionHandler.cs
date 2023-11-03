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
    public class ModifyPermissionHandler : IRequestHandler<ModifyPermissionCommand, bool>
    {
        private readonly IPermissionsApplication _permissionsService;

        public ModifyPermissionHandler(IPermissionsApplication permissionsApplication)
        {
            _permissionsService = permissionsApplication;
        }

        public async Task<bool> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
        {
            return await _permissionsService.ModifyPermission(request.input);
        }
    }
}