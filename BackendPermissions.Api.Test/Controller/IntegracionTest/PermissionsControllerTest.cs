using BackendPermissions.Api.Controllers;
using BackendPermissions.Api.UnitTest.Data;
using BackendPermissions.Application.Interface;
using BackendPermissions.Application.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using BackendPermissions.Application.Model;
using Microsoft.EntityFrameworkCore;
using BackendPermissions.Api.Model;
using Moq;
using BackendPermissions.Infraestructura;
using BackendPermissions.Infraestructura.Repository;

namespace BackendPermissions.Api.UnitTest.Controller.IntegracionTest
{
    public class PermissionsControllerTest
    {
        private readonly IPermissionsApplication _permissionsApplication;
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsControllerTest()
        {
            _permissionsApplication = A.Fake<IPermissionsApplication>();
            _logger = A.Fake<ILogger<PermissionsController>>();
        }

        private async Task<DBContextBackendPermissions> GetDataBaseContext()
        {
            var newOptions = new DbContextOptionsBuilder<DBContextBackendPermissions>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dataBaseContext = new DBContextBackendPermissions(newOptions);
            dataBaseContext.Database.EnsureCreated();
            // populate de database in memory
            if (await dataBaseContext.Permissions.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    dataBaseContext.PermissionTypes.Add(new Infraestructura.Repositor.PermissionTypesEF()
                    {
                        Id = 1,
                        Descripcion = "user"
                    });

                    dataBaseContext.PermissionTypes.Add(new Infraestructura.Repositor.PermissionTypesEF()
                    {
                        Id = 2,
                        Descripcion = "admin"
                    });

                    dataBaseContext.Permissions.Add(new Infraestructura.Repository.PermissionsEF()
                    {
                        Id = 1,
                        NombreEmpleado = "peter",
                        ApellidoEmpleado = "parker",
                        TipoPermiso = 1,
                        FechaPermiso = DateTime.Now
                    });


                    dataBaseContext.Permissions.Add(new Infraestructura.Repository.PermissionsEF()
                    {
                        Id = 2,
                        NombreEmpleado = "john",
                        ApellidoEmpleado = "snow",
                        TipoPermiso = 2,
                        FechaPermiso = DateTime.Now,
                    });

                    await dataBaseContext.SaveChangesAsync();
                }
            }

            return dataBaseContext;

        }

        /// <summary>
        /// Integration testing
        /// </summary>
        /// <returns></returns>
        [Fact]
        private async Task GetPermissions_ReturnFronDataBaseOK()
        {
            /// Arrange
            var contextDb = GetDataBaseContext();
            var name = "peter";                      

            /// Assert
            
        }
    }
}


