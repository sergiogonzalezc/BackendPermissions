using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BackendPermissions.DTO;

namespace BackendPermissions.Application.Model
{
    public class InputModifyPermission
    {
        public int Id { get; set; }

        public string NombreEmpleado { get; set; }
        public string ApellidoEmpleado { get; set; }

        public int TipoPermiso { get; set; }

        public DateTime FechaPermiso { get; set; }

    }
}
