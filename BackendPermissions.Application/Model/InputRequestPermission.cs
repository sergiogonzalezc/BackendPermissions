using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BackendPermissions.DTO;

namespace BackendPermissions.Application.Model
{
    public class InputRequestPermission
    {
        public string NombreEmpleado { get; set; }
        public string ApellidoEmpleado { get; set; }

        public int TipoPermiso { get; set; }       
    }
}
