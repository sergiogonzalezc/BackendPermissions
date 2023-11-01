using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BackendPermissions.Application.Model
{
    public class RequestPermissionDTO
    {
        public bool PermissionOk{ get; set; }

        public DateTime FechaPermiso { get; set; }

    }
}
