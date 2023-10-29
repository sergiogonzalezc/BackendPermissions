using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BackendPermissions.Application.Model
{
    public class PermissionsDTO
    {
        public int Id { get; set; }

        public string NombreEmpleado { get; set; }
        public string ApellidoEmpleado { get; set; }

        public int TipoPermisoCode { get; set; }

        public string TipoPermisoDesc { get; set; }

        public DateTime FechaPermiso { get; set; }

    }
}
