﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPermissions.Application.Model
{
    public class ResultRequestPermissionDTO
    {
        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
