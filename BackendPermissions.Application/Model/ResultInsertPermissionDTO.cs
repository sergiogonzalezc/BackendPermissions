using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BackendPermissions.Application.Model
{
    public class ResultInsertPermissionDTO
    {
        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
