using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BackendPermissions.Application.Model;
using BackendPermissions.Infraestructura;
using BackendPermissions.Application.Interface;
using BackendPermissions.Api.Model;
using static BackendPermissions.Common.Enum;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Utilities;
using System.Configuration;
using BackendPermissions.Application.Business;
using MySqlX.XDevAPI.Common;
using BackendPermissions.Common;

namespace BackendPermissions.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionsApplication _permissionsService;
        private readonly ILogger<PermissionsController> _logger;

        private BackendPermissions.Api.Model.Error err = new BackendPermissions.Api.Model.Error
        {
            Codigo = StatusCodes.Status400BadRequest
        };

        public PermissionsController(IPermissionsApplication permissionsServices, ILogger<PermissionsController> logger)
        {
            _permissionsService = permissionsServices;
            _logger = logger;
            //_producer = producer;
        }


        [HttpGet]
        [Route("GetPermissions")]
        [ProducesResponseType(typeof(List<BackendPermissions.Application.Model.Permissions>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<PermissionsModel> GetPermissions()
        {
            string nameMethod = nameof(GetPermissions);
            try
            {
                _logger.LogInformation("Start...");
                List<PermissionsDTO> result = await _permissionsService.GetPermissions();

                // log message in kafka
                _ = ProducerEventKafka.SendProducerEvent(Guid.NewGuid().ToString(), Common.Enum.CallType.Get.ToString());

                var finalResult = new PermissionsModel
                {
                    Status = Common.Enum.EnumMessage.Succes.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                    Success = true,
                    Message = "ok",
                    DataList = result,
                };

                return finalResult;

            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;

                return new PermissionsModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = arEx.Message
                };
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;
                //return BadRequest(err);

                return new PermissionsModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        [HttpGet]
        [Route("GetPermissionById")]
        [ProducesResponseType(typeof(BackendPermissions.Application.Model.Permissions), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<PermissionsModel> GetPermissionById(int id)
        {
            string nameMethod = nameof(GetPermissions);
            try
            {
                _logger.LogInformation("Start...");
                PermissionsDTO result = await _permissionsService.GetPermissionsById(id);

                // log message in kafka
                _ = ProducerEventKafka.SendProducerEvent(Guid.NewGuid().ToString(), Common.Enum.CallType.Get.ToString());

                var finalResult = new PermissionsModel
                {
                    Status = Common.Enum.EnumMessage.Succes.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                    Success = true,
                    Message = "ok",
                    Data = result,
                    DataList = null,
                };

                return finalResult;

            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;

                return new PermissionsModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = arEx.Message
                };
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;
                //return BadRequest(err);

                return new PermissionsModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Request a Permission
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RequestPermission")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<RequestPermisionModel> RequestPermission([FromBody] InputRequestPermission input)
        {
            string nameMethod = nameof(RequestPermission);

            try
            {
                // log message in kafka
                _ = ProducerEventKafka.SendProducerEvent(Guid.NewGuid().ToString(), Common.Enum.CallType.Request.ToString());

                bool permissionStatus = await _permissionsService.ExistsPermissionByNameAndType(input.NombreEmpleado, input.ApellidoEmpleado);

                var finalResult = new RequestPermisionModel
                {
                    Success = permissionStatus,
                    Message = permissionStatus ? "ok" : "error",
                    Data = null,
                    DataList = null,
                };

                return finalResult;
            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;

                return new RequestPermisionModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = arEx.Message
                };
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;

                return new RequestPermisionModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        /// <summary>
        /// Modify Permission
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("ModifyPermission")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> ModifyPermission([FromBody] InputModifyPermission input)
        {
            string nameMethod = nameof(ModifyPermission);

            try
            {
                // log message in kafka
                _ = ProducerEventKafka.SendProducerEvent(Guid.NewGuid().ToString(), Common.Enum.CallType.Modify.ToString());

                bool result = await _permissionsService.ModifyPermission(new InputModifyPermission()
                {
                    Id = input.Id,
                    NombreEmpleado = input.NombreEmpleado,
                    ApellidoEmpleado = input.ApellidoEmpleado,
                    TipoPermiso = input.TipoPermiso,
                    FechaPermiso = input.FechaPermiso,
                }
                );

                _logger.LogInformation($"Modifiy: {result}");

                return Ok(result);
            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;
                return StatusCode(StatusCodes.Status206PartialContent, err);
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;
                return BadRequest(err);
            }
        }

        /// <summary>
        /// Get the full permission type List
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPermissionTypes")]
        [ProducesResponseType(typeof(List<BackendPermissions.Application.Model.PermissionTypes>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<PermissionTypesModel> GetPermissionTypes()
        {
            string nameMethod = nameof(GetPermissionTypes);
            try
            {
                _logger.LogInformation("Start...");
                List<PermissionTypes> result = await _permissionsService.GetPermissionTypes();

                var finalResult = new PermissionTypesModel
                {
                    Status = Common.Enum.EnumMessage.Succes.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                    Success = true,
                    Message = "ok",
                    DataList = result,
                };

                return finalResult;

            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;

                return new PermissionTypesModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = arEx.Message
                };
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;
                //return BadRequest(err);

                return new PermissionTypesModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        /// <summary>
        /// Get the permission type List by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPermissionTypeById")]
        [ProducesResponseType(typeof(BackendPermissions.Application.Model.PermissionTypes), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<PermissionTypesModel> GetPermissionTypeById(int id)
        {
            string nameMethod = nameof(GetPermissionTypes);
            try
            {
                _logger.LogInformation("Start...");
                PermissionTypes result = await _permissionsService.GetPermissionTypeById(id);
                                
                var finalResult = new PermissionTypesModel
                {
                    Status = Common.Enum.EnumMessage.Succes.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                    Success = true,
                    Message = "ok",
                    Data = result,
                };

                return finalResult;

            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;

                return new PermissionTypesModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = arEx.Message
                };
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;
                //return BadRequest(err);

                return new PermissionTypesModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = ex.Message
                };
            }

        }

        /// <summary>
        /// Insert a new Permission
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertNewPermission")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> InsertNewPermission([FromBody] InputCreatePermission input)
        {
            try
            {
                bool finalResult = await _permissionsService.InsertNewPermission(new InputCreatePermission()
                {
                    NombreEmpleado = input.NombreEmpleado,
                    ApellidoEmpleado = input.ApellidoEmpleado,
                    TipoPermiso = input.TipoPermiso,
                    FechaPermiso = input.FechaPermiso,
                }
                );

                _logger.LogInformation($"Insert New Permission: {finalResult}");

                return Ok(finalResult);
            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameof(InsertNewPermission), "Error!");

                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;
                return StatusCode(StatusCodes.Status206PartialContent, err);
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameof(InsertNewPermission), "Error!");

                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;
                return BadRequest(err);
            }
        }
    }
}
