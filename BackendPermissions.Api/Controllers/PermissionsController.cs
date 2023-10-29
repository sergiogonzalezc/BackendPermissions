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

namespace BackendPermissions.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {

        private readonly IPermissionsApplication _permissionsService;
        private readonly ILogger<PermissionsController> _logger;
        //private readonly IProducer<Null, string> _producer;

        private BackendPermissions.Api.Model.Error err = new BackendPermissions.Api.Model.Error
        {
            Codigo = StatusCodes.Status400BadRequest
        };

        public PermissionsController(IPermissionsApplication permissionsServices, ILogger<PermissionsController> logger/*, IProducer<Null, string> producer*/)
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
        [ProducesResponseType(typeof(List<BackendPermissions.Application.Model.Permissions>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RequestPermission([FromBody] InputRequestPermission input)
        {
            try
            {
                // log message in kafka
                _ = ProducerEventKafka.SendProducerEvent(Guid.NewGuid().ToString(), Common.Enum.CallType.Request.ToString());

                bool finalResult = await _permissionsService.ExistsPermissionByNameAndType(input.NombreEmpleado,input.ApellidoEmpleado,input.TipoPermiso);

                _logger.LogInformation($"Request Permission: {finalResult}");

                return Ok(finalResult);
            }
            catch (ArgumentException arEx)
            {
                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;
                return StatusCode(StatusCodes.Status206PartialContent, err);
            }
            catch (Exception ex)
            {
                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;
                return BadRequest(err);
            }
        }


        /// <summary>
        /// Modify Permission
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("ModifyPermission")]
        [ProducesResponseType(typeof(List<BackendPermissions.Application.Model.Permissions>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ModifyPermission([FromBody] InputModifyPermission input)
        {
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
                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;
                return StatusCode(StatusCodes.Status206PartialContent, err);
            }
            catch (Exception ex)
            {
                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;
                return BadRequest(err);
            }
        }



        /// <summary>
        /// Insert a new Permission
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertNewPermission")]
        [ProducesResponseType(typeof(List<BackendPermissions.Application.Model.Permissions>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InsertNewPermission([FromBody] InputCreatePermission input)
        {
            try
            {
                // log message in kafka
                _ = ProducerEventKafka.SendProducerEvent(Guid.NewGuid().ToString(), Common.Enum.CallType.NewInsertPermission.ToString());

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
                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;
                return StatusCode(StatusCodes.Status206PartialContent, err);
            }
            catch (Exception ex)
            {
                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;
                return BadRequest(err);
            }
        }
    }
}
