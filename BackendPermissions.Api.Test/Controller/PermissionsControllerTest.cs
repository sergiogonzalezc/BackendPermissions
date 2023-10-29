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

namespace BackendPermissions.Api.UnitTest.Controller
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


        [Fact]
        private async Task GetPermissions_ResturnOK()
        {
            /// Arrange
            var controller = new PermissionsController(_permissionsApplication, _logger);

            /// Act
            var result = controller.GetPermissions();

            /// Assert
            result.Should().NotBeNull();
        }


        [Fact]
        private async Task GetPermissions_RequestPermission()
        {
            /// Arrange                        
            var controller = new PermissionsController(_permissionsApplication, _logger);

            InputRequestPermission input = new InputRequestPermission
            {
                NombreEmpleado = "german",
                ApellidoEmpleado = "gonzalez",
                TipoPermiso = 1
            };

            /// Act
            var result = controller.RequestPermission(input);

            /// Assert
            result.Should().NotBeNull();
        }


        [Fact]
        private async Task GetPermissions_ModifyPermission()
        {
            /// Arrange
            var permissionsApplication = A.Fake<ICollection<BackendPermissions.Api.Model.PermissionsModel>>();
            var permissionsList = A.Fake<List<BackendPermissions.Api.Model.PermissionsModel>>();

            var logger = A.Fake<ILogger<PermissionsController>>();

            var controller = new PermissionsController(_permissionsApplication, _logger);

            InputModifyPermission input = new InputModifyPermission
            {
                Id = 1,
                NombreEmpleado = "german",
                ApellidoEmpleado = "gonzalez",
                TipoPermiso = 1
            };

            /// Act
            var result = controller.ModifyPermission(input);

            /// Assert
            result.Should().NotBeNull();

        }
    }
}


