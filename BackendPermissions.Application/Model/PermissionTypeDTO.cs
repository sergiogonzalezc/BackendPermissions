using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BackendPermissions.Application.Model
{
    public class PermissionTypeDTO
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }
    }
}
