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

    /// <summary>
    /// Implement a CQRS handler 
    /// </summary>
    public class GetPermissionByIdHandler : IRequestHandler<GetPermissionByIdQuerys, PermissionsDTO>
    {
        private readonly IPermissionsApplication _permissionsService;

        public GetPermissionByIdHandler(IPermissionsApplication permissionsApplication)
        {
            _permissionsService = permissionsApplication;
        }

        public async Task<PermissionsDTO> Handle(GetPermissionByIdQuerys request, CancellationToken cancellationToken)
        {
            return await _permissionsService.GetPermissionsById(request.id);
        }

    }
}
