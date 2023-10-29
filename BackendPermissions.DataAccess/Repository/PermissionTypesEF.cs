using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPermissions.Infraestructura.Repositor
{
    public class PermissionTypesEF
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
}
