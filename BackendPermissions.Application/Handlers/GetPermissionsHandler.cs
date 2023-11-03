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
    public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuerys, List<PermissionsDTO>>
    {
        private readonly IPermissionsApplication _permissionsService;

        public GetPermissionsHandler(IPermissionsApplication permissionsApplication)
        {
            _permissionsService = permissionsApplication;
        }

        public async Task<List<PermissionsDTO>> Handle(GetPermissionsQuerys request, CancellationToken cancellationToken)
        {
            return await _permissionsService.GetPermissions();
        }

    }
}
