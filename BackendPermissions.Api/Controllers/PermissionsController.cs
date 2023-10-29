using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BackendPermissions.Application.Model;
using BackendPermissions.Infraestructura;
using BackendPermissions.Application.Interface;
using BackendPermissions.Api.Model;
using static BackendPermissions.Common.Enum;
using Microsoft.AspNetCore.Authorization;

namespace BackendPermissions.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {

        private readonly IPermissionsApplication _permissionsService;
        private readonly ILogger<PermissionsController> _logger;

        private Error err = new Error
        {
            Codigo = StatusCodes.Status400BadRequest
        };

        public PermissionsController(IPermissionsApplication permissionsServices, ILogger<PermissionsController> logger)
        {
            _permissionsService = permissionsServices;
            _logger = logger;
        }


        [HttpGet]
        [Route("GetList")]
        [ProducesResponseType(typeof(List<BackendPermissions.Application.Model.Permissions>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<PermissionsModel> GetList()
        {
            try
            {
                _logger.LogInformation("Incio...");
                List<PermissionsDTO> resultado = await _permissionsService.GetPermissions();

                var result = new PermissionsModel
                {
                    Status = Common.Enum.EnumMessage.Succes.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                    Success = true,
                    Message = "ok",
                    DataList = resultado,
                };

                return result;

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

        [HttpPost]
        [Route("InsertPermission")]
        [ProducesResponseType(typeof(List<BackendPermissions.Application.Model.Permissions>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InsertPermission([FromBody] InputCreatePermission input)
        {
            try
            {
                bool resultado = await _permissionsService.InsertPermissions(new InputCreatePermission()
                {
                    NombreEmpleado = input.NombreEmpleado,
                    ApellidoEmpleado = input.ApellidoEmpleado,
                    TipoPermiso = input.TipoPermiso,
                    FechaPermiso = input.FechaPermiso,
                }
                );

                _logger.LogInformation($"Insert: {resultado}");

                return Ok(resultado);
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
