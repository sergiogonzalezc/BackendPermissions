using BackendPermissions.Api.Model;
using BackendPermissions.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPermissions.Api.UnitTest.Data
{
    public class TodoMockData
    {
        public static List<PermissionsModel> GetTodo()
        {
            return new List<PermissionsModel>
            {
                new PermissionsModel
                {
                    Code = "1",
                    DataList = new List<PermissionsDTO>
                    {
                        new PermissionsDTO
                        {
                            Id = 1,
                            NombreEmpleado = "1",
                            ApellidoEmpleado = "1",
                            FechaPermiso = DateTime.Now
                        }
                    }
                }
            };
        }
    }
}
