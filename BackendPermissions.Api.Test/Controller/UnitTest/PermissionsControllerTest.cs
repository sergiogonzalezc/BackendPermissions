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

namespace BackendPermissions.Api.UnitTest.Controller.UnitTest
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

        /// <summary>
        /// Unit testing
        /// </summary>
        /// <returns></returns>
        [Fact]
        private async Task GetPermissions_ReturnOK()
        {
            /// Arrange
            var controller = new PermissionsController(_permissionsApplication, _logger);

            /// Act
            var result = await controller.GetPermissions();

            /// Assert
            result.Should().NotBeNull();
        }

        /// <summary>
        /// Unit testing
        /// </summary>
        /// <returns></returns>
        [Fact]
        private async Task GetPermissions_ReturnOKV2()
        {
            var mockReview = new List<PermissionsModel>
            {
                new PermissionsModel
                {
                    Code = "1",
                    Data = null,
                    Message = "Ok",
                    DataList = new List<PermissionsDTO>
                    {
                       new PermissionsDTO
                       {
                           Id=1,
                           NombreEmpleado="Test",
                           ApellidoEmpleado="Test",
                           FechaPermiso=DateTime.Now,
                       },
                       new PermissionsDTO
                       {
                           Id=2,
                           NombreEmpleado="Test2",
                           ApellidoEmpleado="Test2",
                           FechaPermiso=DateTime.Now,
                       }
                    }
                }
            };

            //var mockPermissionsApplication = new Mock<IPermissionsApplication>();
            //mockPermissionsApplication.Setup(x => x.GetPermissions()).Returns(mockReview);

            /// Arrange
            var controller = new PermissionsController(_permissionsApplication, _logger);

            /// Act
            var result = await controller.GetPermissions();

            /// Assert
            result.Should().NotBeNull();
        }


        /// <summary>
        /// Integration testing
        /// </summary>
        /// <returns></returns>
        [Fact]
        private async Task GetPermissionsFromDataBase_ReturnOK()
        {
            // DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();

            /// Arrange
            var controller = new PermissionsController(_permissionsApplication, _logger);

            /// Act
            var result = await controller.GetPermissions();

            /// Assert
            result.Should().NotBeNull();
        }



        [Fact]
        private async Task GetPermissions_RequestPermission_ReturnOK()
        {
            /// Arrange                        
            var controller = new PermissionsController(_permissionsApplication, _logger);

            InputRequestPermission input = new InputRequestPermission
            {
                NombreEmpleado = "german",
                ApellidoEmpleado = "gonzalez",
                //poPermiso = 1
            };

            /// Act
            var result = await controller.RequestPermission(input);

            /// Assert
            result.Should().NotBeNull();
        }


        [Fact]
        private async Task GetPermissions_ModifyPermission()
        {
            /// Arrange
            var permissionsApplication = A.Fake<ICollection<PermissionsModel>>();
            var permissionsList = A.Fake<List<PermissionsModel>>();

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
            //var result = await controller.ModifyPermission(input);

            /// Assert
            //result.Should().NotBeNull();

        }
    }
}


