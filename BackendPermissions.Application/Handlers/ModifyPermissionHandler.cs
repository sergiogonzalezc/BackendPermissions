using BackendPermissions.Application.Commands;
using BackendPermissions.Application.Interface;
using MediatR;
using Nest;
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
        private readonly ElasticClient _elasticClient;

        public ModifyPermissionHandler(IPermissionsApplication permissionsApplication, ElasticClient elasticClient)
        {
            _permissionsService = permissionsApplication;
            _elasticClient = elasticClient; 
        }

        public async Task<bool> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
        {
            return await _permissionsService.ModifyPermission(_elasticClient, request.input);
        }
    }
}