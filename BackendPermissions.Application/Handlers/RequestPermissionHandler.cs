using BackendPermissions.Application.Commands;
using BackendPermissions.Application.Interface;
using BackendPermissions.Application.Model;
using BackendPermissions.Application.Querys;
using MediatR;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPermissions.Application.Handlers
{
    public class RequestPermissionHandler : IRequestHandler<RequestPermissionCommand, ResultRequestPermissionDTO>
    {
        private readonly IPermissionsApplication _permissionsService;
        private readonly ElasticClient _elasticClient;

        public RequestPermissionHandler(IPermissionsApplication permissionsApplication, ElasticClient elasticClient)
        {
            _permissionsService = permissionsApplication;
            _elasticClient = elasticClient;
        }

        public async Task<ResultRequestPermissionDTO> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
        {
            return await _permissionsService.RequestPermission(_elasticClient, request.input);
        }
    }
}

