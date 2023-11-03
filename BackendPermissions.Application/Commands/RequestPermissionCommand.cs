using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPermissions.Application.Commands
{
    public record InsertPermissionCommand(string NombreEmpleado, string ApellidoEmpleado) : IRequest<bool>
    {
    }
}
