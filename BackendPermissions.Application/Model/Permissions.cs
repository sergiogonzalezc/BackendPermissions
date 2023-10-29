using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BackendPermissions.Application.Model
{
    public class Permissions
    {
        public int Id { get; set; }

        public string NombreEmpleado { get; set; }
        public string ApellidoEmpleado { get; set; }

        public int TipoPermiso { get; set; }

        public DateTime FechaPermiso { get; set; }

    }
}
